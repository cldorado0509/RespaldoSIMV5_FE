namespace SIM.Areas.Tramites.Models
{
    public class DatosTareasFuncionario
    {
        public int CodFuncionario { get; set; }
        public int NroTareasPendientes { get; set; }
        public int NroTareasTerminadas { get; set; }
        public int NroTareasNoAbiertas { get; set; }
        public int NroTareasCopiaPendientes { get; set; }
        public int NroTareasCopiaTerminadas { get; set; }
        public int NroTareasCopiaNoAbiertas { get; set; }
    }
}