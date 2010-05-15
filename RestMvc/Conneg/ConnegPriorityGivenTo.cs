namespace RestMvc.Conneg
{
    /// <summary>
    /// When the server can provide multiple media types that the client
    /// accepts, this enum decides whether we give priority to the client
    /// Accept type ordering, or the server ordering in MediaTypeFormatMap.
    /// According to the spec, the client takes priority, but I added server
    /// priority as a way to work around what I consider to be a bug in
    /// Google Chrome - despite its inability to natively render XML,
    /// Chrome claims to prefer XML over HTML in the Accept header.
    /// </summary>
    public enum ConnegPriorityGivenTo
    {
        /// <summary>
        /// The client ordering of media types in the Accept header takes priority
        /// </summary>
        Client,

        /// <summary>
        /// The server ordering of media types in the MediaTypeFormatMap takes priority
        /// </summary>
        Server
    }
}
