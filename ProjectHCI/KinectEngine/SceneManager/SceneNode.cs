using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ProjectHCI.KinectEngine
{
    public class SceneNode
    {
        private SceneNode parentSceneNode;
        private List<SceneNode> childSceneNodeList;
        private IGameObject sceneNodeGameObject;



        public SceneNode(IGameObject sceneNodeGameObject)
        {
            this.parentSceneNode = null;
            this.sceneNodeGameObject = sceneNodeGameObject;
            this.childSceneNodeList = new List<SceneNode>();
        }



       

        public SceneNode getParent()
        {
            return this.parentSceneNode;
        }

        private void setParent(SceneNode parentSceneNode)
        {
            this.parentSceneNode = parentSceneNode;
        }

        public IGameObject getSceneNodeGameObject()
        {
            return this.sceneNodeGameObject;
        }

        public void addChild(SceneNode childSceneNode)
        {
            Debug.Assert(!this.childSceneNodeList.Contains(childSceneNode), "childSceneNode already present");

            this.childSceneNodeList.Add(childSceneNode);
            childSceneNode.setParent(this);
        }

        public void removeChild(SceneNode childSceneNode)
        {
            Debug.Assert(this.childSceneNodeList.Contains(childSceneNode), "childSceneNode not found present");
            this.childSceneNodeList.Remove(childSceneNode);
        }

        public List<SceneNode> getChildList()
        {
            return new List<SceneNode>(this.childSceneNodeList);
        }







        public static SceneNode getSceneNodeByGameObject(SceneNode rootSceneNode, IGameObject gameObject)
        {
            Debug.Assert(rootSceneNode != null, "expected rootSceneNode != null");
            return SceneNode.recursiveGetSceneNodeByGameObject(rootSceneNode, gameObject);
            
            
        }

        private static SceneNode recursiveGetSceneNodeByGameObject(SceneNode currentSceneNode, IGameObject gameObject)
        {
            IGameObject currentGameObject = currentSceneNode.getSceneNodeGameObject();

            if (SceneNode.equals(currentGameObject, gameObject))
            {
                return currentSceneNode;
            } 
            else
            {
                SceneNode sceneNodeCandidate = null;
                foreach (SceneNode childSceneNode0 in currentSceneNode.getChildList())
                {
                    sceneNodeCandidate = SceneNode.recursiveGetSceneNodeByGameObject(childSceneNode0, gameObject);
                    if (sceneNodeCandidate != null)
                    {
                        break;
                    }
                }

                return sceneNodeCandidate;
            }
        }


        private static bool equals(IGameObject gameObject1, IGameObject gameObject2)
        {
            return (gameObject1 == null && gameObject2 == null) || (gameObject1 != null && gameObject1.Equals(gameObject2));

        }
        

    }
}
