using System.Collections.Generic;
using System.IO;

namespace VirtualAssistantBusinessLogic.SparQL
{
    /// <summary>
    /// Interface for decoding the response from a SparQL connection
    /// </summary>
    public interface IResponseDecoder
    {
        Dictionary<string, Dictionary<string, List<string>>> Decode(Stream stream);
    }
}
