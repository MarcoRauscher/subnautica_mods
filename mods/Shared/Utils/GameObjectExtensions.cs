using System.Collections.Generic;
using UnityEngine;

namespace SharedCode.Utils
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Traverse scene hierarchy topwards (parents) until a GameObject with a specific component is found.
        /// </summary>
        /// <typeparam name="TComponent">Component to look for</typeparam>
        /// <param name="instance">GameObject whose parent hierarchy shall be checked</param>
        /// <returns></returns>
        public static GameObject FindParentWithComponent<TComponent>(this GameObject instance)
        {
            GameObject parent = instance.transform.parent.gameObject;

            while (parent != null)
            {
                if (parent.GetComponent<TComponent>() != null)
                    return parent;

                parent = instance.transform.parent.gameObject;
            }

            return null;
        }
    }
}
