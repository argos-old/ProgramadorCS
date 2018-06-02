namespace ProgramadorCS
{
    #region " Enumeraciones "

    public enum tipoAccion
    {
        Anular,
        Apagar,
        Reiniciar,
        CerrarSesion,
        Suspender,
        Hibernar,
        Bloquear,
        Avisar,
        Ejecutar,
        ApagarInmediato,
        ReiniciarAppsRegistradas,
        ReiniciarMenuOpciones,
        EnviarMail
    }

    enum tipoCondicion
    {
        A1Hora,
        CuentaAtras,
        CargaEquipo,
        EventosRed,
        Energia,
        Unidades
    }

    enum tabRibbon
    {
        Principal,
        Inmediato,
        Ver,
        Apariencia,
        Configuracion
    }

    enum opcionCarga
    {
        RAMPorcentual,
        RAMDatos,
        CPU,
        Temperatura
    }

    enum opcionEnergia
    {
        Alimentacion,
        PlanEnergia,
        Bateria
    }
    public enum Magnitud
    {
        Bytes,
        KB,
        MB,
        GB,
        TB
    }

    public enum Estado
    {
        Parado,
        Pausado,
        Iniciado
    }

    #endregion
}
