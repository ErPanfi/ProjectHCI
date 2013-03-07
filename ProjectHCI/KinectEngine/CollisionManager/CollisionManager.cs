using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Diagnostics;

namespace ProjectHCI.KinectEngine
{
    public class CollisionManager : ICollisionManager
    {

        private ISceneBrain sceneBrain;
        private ISet<KeyValuePair<GameObjectTypeEnum, GameObjectTypeEnum>> typeEnumCollidablePairSet; 


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sceneBrain"></param>
        public CollisionManager(ISceneBrain sceneBrain)
        {
            this.sceneBrain = sceneBrain;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<IGameObject, IGameObject>> createCollisionList()
        {


            List<IGameObject> userFriendlyGameObjectList = this.sceneBrain.getCollaidableGameObjectList(GameObjectTypeEnum.FriendlyObject);

            List<IGameObject> notUserFriendlyGameObjectList = this.sceneBrain.getCollaidableGameObjectList(GameObjectTypeEnum.UnfriendlyObject);
            List<IGameObject> userGameObjectList = this.sceneBrain.getCollaidableGameObjectList(GameObjectTypeEnum.UserObject);


            IGameObject userGameObject = null;
            if (userGameObjectList.Count > 0)
            {
                Debug.Assert(userGameObjectList.Count == 1, "expected exactly one userGameObject into the scene, multiplayer not supported yet");
                userGameObject = userGameObjectList.ElementAt(0);
            }
            



            /*return object*/
            List<KeyValuePair<IGameObject, IGameObject>> gameObjectKeyValuePairList = new List<KeyValuePair<IGameObject, IGameObject>>();

            notUserFriendlyGameObjectList.ForEach(delegate(IGameObject notUserFriendlyGameObject)
            {
                //manage collision <user, not user friendly>
                Point[] p = collisionDetect(userGameObject.getGeometry(), notUserFriendlyGameObject.getGeometry());
                if (p.Length > 0)
                {
                    gameObjectKeyValuePairList.Add(new KeyValuePair<IGameObject, IGameObject>(userGameObject, notUserFriendlyGameObject));
                }

                //manage collision <user friendly, not user friendly>
                userFriendlyGameObjectList.ForEach(delegate(IGameObject userFriendlyGameObject)
                {
                    Point[] p2 = collisionDetect(userFriendlyGameObject.getGeometry(), notUserFriendlyGameObject.getGeometry());
                    if (p2.Length > 0)
                    {
                        gameObjectKeyValuePairList.Add(new KeyValuePair<IGameObject, IGameObject>(userGameObject, userFriendlyGameObject));
                    }
                });

            });
            //manage collision <user, user friendly>
            userFriendlyGameObjectList.ForEach(delegate(IGameObject userFriendlyGameObject)
            {          
                Point[] p = collisionDetect(userGameObject.getGeometry(), userFriendlyGameObject.getGeometry());
                if (p.Length > 0)
                {
                    gameObjectKeyValuePairList.Add(new KeyValuePair<IGameObject, IGameObject>(userGameObject, userFriendlyGameObject));
                }
            });
            return gameObjectKeyValuePairList;
        }

        /// <summary>
        /// test collision between two Geometry object
        /// </summary>
        /// <param name="firstGeometry">first Geometry</param>
        /// <param name="secondGeometry">second Geometry</param>
        /// <returns>intersection Points</returns>
        private Point[] collisionDetect(Geometry firstGeometry, Geometry secondGeometry)
        {
            Geometry firstPathGeometry = firstGeometry.GetWidenedPathGeometry(new Pen(Brushes.Black, 1.0)); //retrieve PathGeometry class, inherit from Geometry Class
            Geometry secondPathGeometry = secondGeometry.GetWidenedPathGeometry(new Pen(Brushes.Black, 1.0));

            CombinedGeometry combGeometry = new CombinedGeometry(GeometryCombineMode.Intersect, firstPathGeometry, secondPathGeometry); //inherit from Geometry
            PathGeometry combPathGeometry = combGeometry.GetFlattenedPathGeometry();

            /*code below retrieve all intersection points of 2 geometry*/
            //combPathGeometry.Figures rappresenta una serie di piccoli segmenti geometrici che compongono la geometria
            Point[] intersectionPoints = new Point[combPathGeometry.Figures.Count];
            //ho inizializzato un'array di punti, però bisogna ancora trovare le coordinate di ognuno di essi
            for (int i = 0; i < combPathGeometry.Figures.Count; i++)
            {
                //crea una piccola geometria a partire dal segmento trovato e ritorna il suo bounding box rettangolare
                Rect fig = new PathGeometry(new PathFigure[] { combPathGeometry.Figures[i] }).Bounds;
                //trova il punto centrale a partire da questo rettangolo
                intersectionPoints[i] = new Point(fig.Left + fig.Width / 2.0, fig.Top + fig.Height / 2.0);
            }
            return intersectionPoints;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeCollidablePairSet"></param>
        public void setCollisionToHandle(ISet<KeyValuePair<GameObjectTypeEnum, GameObjectTypeEnum>> typeEnumCollidablePairSet)
        {
            this.typeEnumCollidablePairSet = typeEnumCollidablePairSet;
        }



    }



}
