using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectHCI.KinectEngine;
using System.Windows.Media;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace ProjectHCI.ReverseFruitNinja
{
    public class FormattedTextGameObject : GameObject
    {

        private String text;
        private bool isTextChanged;



        public FormattedTextGameObject(double xPosition,
                                  double yPosition,
                                  String text)
        {
            
            base._xPosition = xPosition;
            base._yPosition = yPosition;
            base._boundingBoxGeometry = null;
            base._extraData = null;
            base._uid = Guid.NewGuid().ToString();
            base._gameObjectTag = Tags.DEBUG_TAG;


            base._image = this.createImageFromText(text);
            this.text = text;

            this.isTextChanged = false;
        }



        private Image createImageFromText(String stringText)
        {
            FormattedText formattedText = new FormattedText(stringText,
                                                            CultureInfo.CurrentCulture, 
                                                            FlowDirection.LeftToRight, 
                                                            new Typeface("Arial"),          // the font... can be a custom font
                                                            100,                            // font size in em
                                                            Brushes.Black);                 //unused param, we create a geometry from this FormattedText


            Geometry textGeometry = formattedText.BuildGeometry(new System.Windows.Point(0, 0));

            GeometryDrawing geometryDrawing = new GeometryDrawing();
            geometryDrawing.Geometry = textGeometry;

            //the glyph's body color, can be a single color, a gradient or an image
            geometryDrawing.Brush = new LinearGradientBrush(Colors.PaleVioletRed,
                                                            Color.FromRgb(204, 204, 255),
                                                            new Point(0, 0),
                                                            new Point(1, 1));

            //the glyph's border color, can be a single color, a gradient or an image
            geometryDrawing.Pen = new Pen(Brushes.Indigo, 3);

            //create an imageSource
            DrawingImage drawingImage = new DrawingImage(geometryDrawing);

            Image image = new Image();
            image.Source = drawingImage;
            

            return image;            
        }



        public void setText(String stringText)
        {
            this.text = stringText;
            this.isTextChanged = true;
        }



        public override void update(int deltaTimeMillis)
        {
            // do nothing
        }




        public override bool isCollidable()
        {
            return false;
        }

        public override bool isDead()
        {
            return false;
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
            throw new NotImplementedException();
        }

        public override void onCollisionExitDelegate(IGameObject otherGameObject)
        {
            throw new NotImplementedException();
        }
    }
}
