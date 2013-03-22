using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProjectHCI.KinectEngine
{
    class FakeSpawner : ISpawnerManager
    {


        private bool firstTime;


        public FakeSpawner()
        {
            this.firstTime = true;
        }



        public void awaken()
        {
            if (firstTime)
            {
                ISceneManager sceneManager = GameLoop.getSceneManager();

                Random random = new Random();


                { //user creation scope
                    Image image = new Image();
                    image.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/shark.png"));
                    image.Height = 200;
                    image.Width = 200;

                    Geometry boundingBoxGeometry = new EllipseGeometry(new Rect(new Point(50, 60), new Point(150, 160)));

                    IGameObject userGameObject = new HeadUserGameObject(400, 400, boundingBoxGeometry, image, SkeletonSmoothingFilter.MEDIUM_SMOOTHING_LEVEL);
                    sceneManager.addGameObject(userGameObject, null);
#if DEBUG
                    sceneManager.addGameObject(new BoundingBoxViewrGameObject(userGameObject), userGameObject);
#endif
                }
                



                for (int index = 1; index <= 5; index++)
                {


                    

                    { //friendly game object scope

                        IGameObject gameObject0 = null;

                        {

                            Image image = new Image();
                            image.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/skype.png"));
                            image.Height = 100;
                            image.Width = 100;

                            Geometry boundingBoxGeometry = new EllipseGeometry(new Rect(new Point(10, 10), new Point(90, 90)));

                            gameObject0 = new UserFriendlyGameObject(100 * index, 0, boundingBoxGeometry, image, random.Next(2000, 10000));
                            sceneManager.addGameObject(gameObject0, null);
#if DEBUG
                            sceneManager.addGameObject(new BoundingBoxViewrGameObject(gameObject0), gameObject0);
#endif
                        }


                        {
                            Image image = new Image();
                            image.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/skype.png"));
                            image.Height = 50;
                            image.Width = 50;

                            Geometry boundingBoxGeometry = new EllipseGeometry(new Rect(new Point(10, 10), new Point(30, 30)));


                            IGameObject gameObject1 = new UserFriendlyGameObject(100, 100, boundingBoxGeometry, image, random.Next(2000, 10000));

                            sceneManager.addGameObject(gameObject1, gameObject0);
#if DEBUG
                            sceneManager.addGameObject(new BoundingBoxViewrGameObject(gameObject1), gameObject1);
#endif
                        }

                    }




                    {// unfriendly game object scope


                        Image image = new Image();
                        image.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/slash_horiz.png"));
                        image.Height = 29;
                        image.Width = 128;

                        
                        Geometry boundingBoxGeometry = new RectangleGeometry(new Rect(new Point(-5, 5), new Point(300, 20)));

                        IGameObject gameObject0 = new NotUserFriendlyGameObject(600, 150 * index, boundingBoxGeometry, image, 5000, 3500);
                        sceneManager.addGameObject(gameObject0, null);
#if DEBUG
                        sceneManager.addGameObject(new BoundingBoxViewrGameObject(gameObject0), gameObject0);
#endif
                    }



                }

                firstTime = false;
            }
            
        }

        public List<IGameObject> getSpawnedObjects()
        {
            throw new Exception("The method or operation is not implemented.");
        }


    }
}
