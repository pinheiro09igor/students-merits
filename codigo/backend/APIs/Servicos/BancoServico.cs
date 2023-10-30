using APIs.Contexto;
using APIs.Modelos.Dtos;
using APIs.Modelos.Entidade;
using APIs.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace APIs.Servicos;

public class BancoServico : IBancoRepositorio
{
    private readonly AppDbContexto _contexto;
    public BancoServico(AppDbContexto contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> Transferir(RealizarTransferenciaDto dto)
    {
        var contaOrigem = await _contexto.Contas
            .FirstOrDefaultAsync(c => c.Identificador.Equals(dto.IdentificadorContaOrigem));
        var contaDestino = await _contexto.Contas
            .FirstOrDefaultAsync(c => c.Identificador.Equals(dto.IdentificadorContaDestino));
        
        if (!ValidarTransferencia(contaOrigem, contaDestino, dto.Valor)) 
            return false;
        
        await using var transacao = await _contexto.Database.BeginTransactionAsync();

        try
        {
            contaOrigem.SaldoBancario -= dto.Valor;
            contaDestino.SaldoBancario += dto.Valor;

            _contexto.Contas.UpdateRange(contaOrigem, contaDestino);
            await _contexto.TransferenciaBancarias.AddAsync(new TransferenciaBancaria
            {
                Id = Guid.NewGuid().ToString(),
                IdentificadorContaOrigem = dto.IdentificadorContaOrigem,
                IdentificadorContaDestino = dto.IdentificadorContaDestino,
                Valor = dto.Valor,
                DataTransferencia = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
            });
            var resposta = await _contexto.SaveChangesAsync();

            await transacao.CommitAsync();
            if (resposta != 0) return true;
        }
        catch
        {
            await transacao.RollbackAsync();
        }
        return false;
    }

    public async Task<ExtratoBancario> GerarExtrato(string identificador)
    {
        var cliente = await _contexto.Usuarios
            .FirstOrDefaultAsync(u => u.Identificador.Equals(identificador));
        var transferencias = await _contexto.TransferenciaBancarias.ToListAsync();
        
        var transferenciaOrigem = transferencias
            .FindAll(e => e.IdentificadorContaOrigem.Equals(identificador));
        var transferenciaDestino = transferencias
            .FindAll(e => e.IdentificadorContaDestino.Equals(identificador));

        transferencias = new List<TransferenciaBancaria>();
        transferencias.AddRange(transferenciaOrigem);
        transferencias.AddRange(transferenciaDestino);

        var transferenciasDto = new List<MostrarTransferenciaDto>();
        foreach (var transferencia in transferencias)
        {
            var clienteOrigem = await _contexto.Usuarios.FirstOrDefaultAsync(c =>
                c.Identificador.Equals(transferencia.IdentificadorContaOrigem));
            var clienteDestino = await _contexto.Usuarios.FirstOrDefaultAsync(c =>
                c.Identificador.Equals(transferencia.IdentificadorContaDestino));
            
            transferenciasDto.Add(new MostrarTransferenciaDto
            {
                NomeUsuarioOrigem = clienteOrigem.Nome,
                IdentificadorUsuarioOrigem = clienteOrigem.Identificador,
                NomeUsuarioDestino = clienteDestino.Nome,
                IdentificadorContaDestino = clienteDestino.Identificador,
                Valor = transferencia.Valor,
                DataTransferencia = transferencia.DataTransferencia,
            });
        }
        
        return cliente is null ? null : new ExtratoBancario
        {
            Nome = cliente.Nome,
            Identificador = cliente.Identificador,
            Transferencias = transferenciasDto
        };
    }

    private static bool ValidarTransferencia(ContaBancaria contaOrigem, ContaBancaria contaDestino, double valorTransferencia)
    {
        if (contaOrigem is null || contaDestino is null) return false;
        return valorTransferencia != 0 && !(contaOrigem.SaldoBancario < valorTransferencia);
    }
}