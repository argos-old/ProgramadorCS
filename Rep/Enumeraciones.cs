namespace Rep 
{ 

    enum TipoAvisoSonoro
    {
        Bip,
        Gallo,
        AlarmaIncendios,
        Aplausos,
        Bip2,
        Bip3,
        Burro,
        Campanillas,
        CorazonMonitorizado,
        CorazonLatiendo,
        DespertadorDigital,
        DespertadorAntiguo,
        DoceCampanadas,
        LlamadaEnterprise,
        Metralleta,
        RisaBebe,
        RisaFemenina,
        RisaMasculina,
        RitmoPercusion1,
        RitmoPercusion2,
        RitmoPercusion3,
        RitmoPercusion4,
        RitmoMilitar,
        RitmoRedoble,
        RitmoTimbales,
        Robot,
        SirenaMaderos,
        TelefonoAntiguo,
        TelefonoDigital,
        TicTac,
        TemaPersonal
    }

    public enum EstadoReproductor
    {
        Parado,
        Reproduciendo,
        Pausado
    }

    public enum ExtensionAudio
    {
        Wav,
        Mp3,
        Aiff,
        Otros
    }

    public enum TipoLectorNAudio
    {
        AudioFileReader,
        Mp3Reader,
        WavReader,
        AiffReader,
    }
}
