using Mirror;
using Mirror.Weaver.Tests.Extra;
using UnityEngine;

namespace GeneratedReaderWriter.CreatesForInheritedFromScriptableObject
{
    public class CreatesForInheritedFromScriptableObject : NetworkBehaviour
    {
        [ClientRpc]
        public void RpcDoSomething(DataScriptableObject data)
        {
            // empty
        }
    }

    public class DataScriptableObject :ScriptableObject
    {
        public int usefulNumber;
    }
}
