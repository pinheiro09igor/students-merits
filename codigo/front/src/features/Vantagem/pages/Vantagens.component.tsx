import { useQuery, useQueryClient } from "@tanstack/react-query";
import React, { useEffect } from "react";
import { VantagemForm } from "../components/VantagemForm";
import { VantagemService } from "../services";
import { IVantagem } from "../services/interfaces";
import { VantagemCard } from "../components";

const modalFormId: string = "vantagemForm";

export const Vantagens = () => {
   const [selectedVantagemId, setSelectedVantagemId] = React.useState<
      string | null
   >(null);
   const id = localStorage.getItem("id");
   const tipo = localStorage.getItem("tipo");

   const queryClient = useQueryClient();
   queryClient.invalidateQueries(["vantagens"]);

   const getListaVantagens = async (): Promise<IVantagem[]> => {
      try {
         const vantagens = await VantagemService.getAllVantagens(
            tipo === "empresa" ? id! : ""
         );
         console.log(vantagens);
         return vantagens;
      } catch (error) {
         return [];
      }
   };

   const { data, refetch } = useQuery<IVantagem[]>({
      queryKey: ["vantagens"],
      queryFn: getListaVantagens,
   });

   const onClickResgatar = async (id: string) => {
      try {
         console.log(id);
         await VantagemService.resgatarVantagem(id);
         queryClient.invalidateQueries(["vantagens"]);
      } catch (error) {
         console.log(error);
      }
   };

   return (
      <>
         <div className="flex flex-col grow h-full">
            {tipo === "empresa" && (
               <div className="flex mx-12 mt-12 justify-end">
                  <label
                     htmlFor={modalFormId}
                     className="btn btn-primary shadow"
                     onClick={() => {
                        setSelectedVantagemId(null);
                     }}
                  >
                     Criar
                  </label>
               </div>
            )}
            <div
               className={`flex flex-col ${
                  data?.length === 0 && "justify-center"
               } mx-12 my-6 h-full`}
            >
               {(data?.length === 0 || data === undefined) && (
                  <div className="flex justify-center items-center p-2">
                     <p className="text-gray-500">
                        Nenhuma vantagem encontrada
                     </p>
                  </div>
               )}
               {data?.length !== 0 && (
                  <div className="flex flex-wrap gap-4 overflow-y-auto">
                     {data?.map((vantagem) => (
                        <label key={vantagem.id} htmlFor={modalFormId}>
                           <VantagemCard
                              id={vantagem.id!}
                              title={vantagem.nome}
                              description={vantagem.descricao}
                              image={vantagem.foto}
                              value={vantagem.valor}
                              onClick={() => {
                                 setSelectedVantagemId(vantagem.id!);
                              }}
                              onClickResgatar={onClickResgatar}
                              isAluno={tipo === "aluno"}
                              resgatada={vantagem.resgatadaPor.length == 0}
                           />
                        </label>
                     ))}
                  </div>
               )}
            </div>
         </div>
         {tipo === "empresa" && (
            <VantagemForm
               modalId={modalFormId}
               refetchVantagens={refetch}
               id={selectedVantagemId}
            />
         )}
      </>
   );
};
