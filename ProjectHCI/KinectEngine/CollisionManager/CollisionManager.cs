﻿using System;
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

        //private ISceneBrain sceneBrain;
        private ISet<KeyValuePair<GameObjectTypeEnum, GameObjectTypeEnum>> typeEnumCollidablePairSet;
        private List<KeyValuePair<IGameObject, IGameObject>> gameObjectKeyValuePairList;

        /// <summary>
        /// 
        /// </summary>
        public CollisionManager()
        {
            this.gameObjectKeyValuePairList = new List<KeyValuePair<IGameObject, IGameObject>>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<IGameObject, IGameObject>> createCollisionList()
        {
            ISceneManager sceneManager = GameLoop.getSceneManager();
            if (gameObjectKeyValuePairList.Count > 0)
            {
                gameObjectKeyValuePairList.Clear();
            }

            foreach (KeyValuePair<GameObjectTypeEnum, GameObjectTypeEnum> gameObjectKeyValuePair in typeEnumCollidablePairSet)
            {
                List<IGameObject> firstGameObjectList = sceneManager.getCollaidableGameObjectList(gameObjectKeyValuePair.Key);
                List<IGameObject> secondGameObjectList = sceneManager.getCollaidableGameObjectList(gameObjectKeyValuePair.Value);
                firstGameObjectList.ForEach(delegate(IGameObject gameObjectInFirstList)
                {
                    secondGameObjectList.ForEach(delegate(IGameObject gameObjectInSecondList)
                    {
                        Point[] p = collisionDetect(gameObjectInFirstList.getGeometry(), gameObjectInSecondList.getGeometry());
                        if (p.Length > 0)
                        {
                            gameObjectKeyValuePairList.Add(new KeyValuePair<IGameObject, IGameObject>(gameObjectInFirstList, gameObjectInSecondList));
                        }
                    });
                });

            }
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
        /// initialize set of object's pair inside collision manager
        /// </summary>
        /// <param name="typeCollidablePairSet">set of object's pair</param>
        public void setCollisionToHandle(ISet<KeyValuePair<GameObjectTypeEnum, GameObjectTypeEnum>> typeEnumCollidablePairSet)
        {
            this.typeEnumCollidablePairSet = typeEnumCollidablePairSet;
        }



    }



}
