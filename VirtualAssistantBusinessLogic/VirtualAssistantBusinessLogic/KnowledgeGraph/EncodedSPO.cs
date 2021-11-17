using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAssistantBusinessLogic.KnowledgeGraph
{
    /// <summary>
    /// Class used to store the encoded SPOs. It contains a Triplet and a Name string
    /// since some encoded SPOs have the form:
    ///     ?NAME predicate object  (The triplet)
    ///     ?NAME ...               (The name)
    /// An example of an encoding that require a triplet is getting all nodes that are
    /// labeled with a string, like when searching for "Donald Trump"
    /// If the triplet is not necessary for the encoding the triplet will simply be ""
    /// and the name can be used directly.
    /// </summary>
    public class EncodedSPO
    {
        public EncodedSPO(string triplet, string name)
        {
            Triplet = triplet;
            Name = name;
        }
        public string Triplet { get; set; }
        public string Name { get; set; }
    }
}
