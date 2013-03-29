﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectHCI.KinectEngine;
using System.Windows.Media;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using ProjectHCI.ReverseFruitNinja;

namespace ProjectHCI.KinectEngine
{
    public class FormattedTextGameObject : GameObject
    {

        private String text;
        private bool isTextChanged;

        protected int timeToLive;
        protected bool isTemporary;


        /// <summary>
        /// Base constructor for formatted text object
        /// </summary>
        /// <param name="xPosition">X-axis position of text upper left corner</param>
        /// <param name="yPosition">Y-axis position of text upper left corner</param>
        /// <param name="text">The text to display</param>
        /// <param name="timeToLive">If a positive value is given the text will last only the given time in milliseconds.Otherwise it will be permanentely displayed.</param>
        public FormattedTextGameObject(double xPosition,
                                       double yPosition,
                                       String text,
                                       int timeToLive)
        {
            
            base._xPosition = xPosition;
            base._yPosition = yPosition;
            base._extraData = null;
            base._uid = Guid.NewGuid().ToString();
            base._gameObjectTag = Tags.UI_TAG;


            base._image = this.createImageFromText(text);
            base._boundingBoxGeometry = new RectangleGeometry(((DrawingImage)base._image.Source).Drawing.Bounds);

            this.isTextChanged = false;
            this.timeToLive = timeToLive;
            this.isTemporary = (timeToLive > 0);
        }

        protected virtual Image createImageFromText(String stringText)
        {
            //create an imageSource
            Image image = new Image();
            image.Source = new DrawingImage(this.textGeometryDrawing(this.formatText(stringText)));
            

            return image;            
        }

        protected virtual FormattedText formatText(string stringText)
        {

            #region miraculous code for embedded font integration - By DC

            //Typeface typeFace = new Typeface(new FontFamily(new Uri("pack://application:,,,/"), "./Resources/#Made in China"),
            //                                 FontStyles.Normal,
            //                                 FontWeights.Medium,
            //                                 FontStretches.Normal);

            #endregion

            return new FormattedText(stringText,
                                     CultureInfo.CurrentCulture,
                                     FlowDirection.LeftToRight,
                                     new Typeface("Arial"),         // the font... can be a custom font
                                     60,                            // font size in em
                                     Brushes.Black);                //unused param, we create a geometry from this FormattedText
        }

        protected virtual GeometryDrawing textGeometryDrawing(FormattedText formattedText)
        {
            GeometryDrawing geometryDrawing = new GeometryDrawing();
            geometryDrawing.Geometry = formattedText.BuildGeometry(new System.Windows.Point(0, 0));

            //the glyph's body color, can be a single color, a gradient or an image
            /*
            geometryDrawing.Brush = new LinearGradientBrush(Colors.PaleVioletRed,
                                                            Color.FromRgb(204, 204, 255),
                                                            new Point(0, 0),
                                                            new Point(1, 1));
            */
            geometryDrawing.Brush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            //the glyph's border color, can be a single color, a gradient or an image
            geometryDrawing.Pen = new Pen(Brushes.Black, 1);

            return geometryDrawing;
        }

        public void setText(String stringText)
        {
            this.isTextChanged = true;
            this.text = stringText;
        }



        public override void update(int deltaTimeMillis)
        {
            if (timeToLive > 0)
                timeToLive -= deltaTimeMillis;
        }

        public override bool isCollidable()
        {
            return false;
        }

        public override bool isDead()
        {
            return (isTemporary && timeToLive <= 0);
        }

        public override void onRendererDisplayDelegate()
        {
            ISceneManager sceneManager = GameLoop.getSceneManager();
            sceneManager.canvasDisplayImage(this, 100);         
        }

        public override void onRendererUpdateDelegate()
        {
            if (this.isTextChanged)
            {
                base._image = this.createImageFromText(this.text);
                base._boundingBoxGeometry = new RectangleGeometry(((DrawingImage)base._image.Source).Drawing.Bounds);

                ISceneManager sceneManager = GameLoop.getSceneManager();
                sceneManager.canvasUpdateImage(this);

                this.isTextChanged = false;
            }
        }

        public override void onRendererRemoveDelegate()
        {
            ISceneManager sceneManager = GameLoop.getSceneManager();
            sceneManager.canvasRemoveImage(this);
        }

        
        public override void onCollisionEnterDelegate(IGameObject otherGameObject)
        {
        }

        public override void onCollisionExitDelegate(IGameObject otherGameObject)
        {
        }
    }
}
