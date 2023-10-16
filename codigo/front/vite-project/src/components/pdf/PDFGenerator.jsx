import React from 'react';

import { Document, Page, Text, View, StyleSheet, PDFDownloadLink, } from '@react-pdf/renderer';

import moment from 'moment';
import 'moment/locale/pt-br';

const styles = StyleSheet.create({
  page: {
    flexDirection: 'row',
    backgroundColor: '#E4E4E4',
  },
  section: {
    margin: 10,
    padding: 10,
    flexGrow: 1,
  },
  transacoesTitle: {
    marginTop: 20,
    fontSize: 15,
  },
  transacoesText: {
    fontSize: 10,
    marginBottom: 10,
    marginTop: 10,
  },
  sameDataTransacao: {
    display: 'flex',
    flexDirection: 'row',
    justifyContent: 'space-between',
    borderBottom: 1,
    borderTop: 1,
    borderColor: "black",
  },
  pageTitle: {
    display: 'flex',
    flexDirection: 'column',
    justifyContent: 'center',
    alignItems: 'center',
    gap: 5,
    marginBottom: 20,
    fontSize: 20,
  },
  lowText: {
    fontSize: 12,
  },
  boldText: {
    fontWeight: 'bold',
    fontSize: 12,
  },
  mediumText: {
    fontSize: 13,
  },
  spaceBetween: {
    display: 'flex',
    flexDirection: 'row',
    justifyContent: 'space-between',
  },
  center: {
    display: 'flex',
    flexDirection: 'row',
    justifyContent: 'center',
  },
  viewSaldo: {
    display: 'flex',
    flexDirection: 'column',
    justifyContent: 'center',
    alignItems: 'center',
    gap: 5,
    marginTop: 20,
  },
  dataUser: {
    borderBottom: 1,
    borderTop: 1,
    borderColor: "black",
    padding: 10,
  },
  textData: {
    marginTop: 20,
    marginBottom: 20,
    fontSize: 15,
  },
  dataUserType: {
    marginBottom: 10,
    fontSize: 12,
  }
});

const formatDate = (dateString) => {
    return moment(dateString).locale('pt-br').format('DD [de] MMMM [de] YYYY');
};

const tipoTransacao = (tipoTransacao , tipoUsuario) => {
    if (tipoTransacao === 'transferencia') {
      if(tipoUsuario === 'Aluno')
        return 'Transferência para o aluno'
      else if(tipoUsuario === 'Professor')
        return 'Transferência para o professor'
      else 
        return 'Transferência para a empresa'
    }
    if (tipoTransacao === 'recebimento') {
        if(tipoUsuario === 'Aluno')
          return 'Recebimento do aluno'
        else if(tipoUsuario === 'Professor')
          return 'Recebimento do professor'
        else 
          return 'Recebimento da empresa'
      }
};

const sinalTransacao = (tipoTransacao) => {
    if(tipoTransacao === 'transferencia'){
        return '-'
    }
    if (tipoTransacao === 'recebimento'){
        return '+'
    }
}

const PDFGenerator = ({ transacoes, saldo, user }) => {
    const MyDocument = () => {
        const reversedTransacoes = [...transacoes].reverse();
        const groupedData = {};
        reversedTransacoes.forEach((transacao) => {
        const formattedDate = formatDate(transacao.data);
        if (groupedData[formattedDate]) {
            groupedData[formattedDate].push(transacao);
        } else {
            groupedData[formattedDate] = [transacao];
        }
    });

    return (
      <Document>
        <Page style={styles.page}>
          <View style={styles.section}>
            <View style={styles.pageTitle}>
                <Text>Extrato</Text>
                <Text style={styles.boldText}>Data de emissão: {moment().format('DD/MM/YYYY')}</Text>
            </View>
            <View style={styles.dataUser}>
                <View>
                    <Text style={styles.dataUserType}>Dados do {user.pessoa.tipo === 'Aluno' ? 'Aluno' : user.pessoa.tipo === 'Professor' ? 'Professor' : 'Usuário'}:</Text>
                    <Text style={styles.lowText}>Nome: {user.pessoa.nome}</Text>
                    <Text style={styles.lowText}>Email: {user.pessoa.email}</Text>
                </View>
                {
                    user.pessoa.tipo === 'Aluno' ?
                    <View>
                        <Text style={styles.lowText}>Instituição de ensino: {user.instituicaoEnsino.nome}</Text>
                        <Text style={styles.lowText}>Curso: {user.curso}</Text>
                    </View>
                    :
                    <View>
                        <Text style={styles.lowText}>Instituição de ensino: {user.instituicaoEnsino.nome}</Text>
                        <Text style={styles.lowText}>Curso: {user.curso}</Text>
                        <Text style={styles.lowText}>Departamento: {user.departamento}</Text>
                    </View>
                }
            </View>
            <View style={styles.viewSaldo}>
                <Text style={styles.mediumText}>Saldo atual</Text>
                <Text style={styles.mediumText}>{saldo} moedas</Text>
            </View>
            <View>
                <Text style={styles.transacoesTitle}>Transações</Text>
            </View>
            {Object.keys(groupedData).map((formattedDate, index) => (
              <View key={index}>
                <Text style={styles.textData}>{formattedDate}</Text>
                {groupedData[formattedDate].map((transacao, index) => (
                <View key={index} style={styles.sameDataTransacao}>
                    <Text style={styles.transacoesText}>{tipoTransacao(transacao.tipo, transacao.destino.tipo)} {transacao.destino.nome}. Descrição: {transacao.descricao}.</Text>
                    <Text style={styles.transacoesText}>{sinalTransacao(transacao.tipo)} {transacao.valor}</Text>
                </View>
                ))}
              </View>
            ))}
          </View>
        </Page>
      </Document>
    );
  };

  return (
    <div>
      <PDFDownloadLink document={<MyDocument />} fileName="extrato.pdf">
        {({ blob, url, loading, error }) =>
          loading ? 'Carregando o PDF...' : 'Baixar PDF'
        }
      </PDFDownloadLink>
    </div>
  );
};

export default PDFGenerator;