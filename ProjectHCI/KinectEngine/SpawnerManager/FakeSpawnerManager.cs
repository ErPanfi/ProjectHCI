using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Threading;

namespace ProjectHCI.KinectEngine
{
    public class FakeSpawnerManager : ISpawnerManager
    {

        private ISceneBrain sceneBrain;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sceneBrain"></param>
        public FakeSpawnerManager(ISceneBrain sceneBrain)
        {
            this.sceneBrain = sceneBrain;
        }


        /// <summary>
        /// 
        /// </summary>
        public void awaken()
        {

            int maxNumberOfChopAllowed = this.sceneBrain.getMaxNumberOfChopAllowed();
            int maxNumberOfUserFriendlyGameObjectAllowed = this.sceneBrain.getMaxNumberOfUserFriendlyGameObjectAllowed();
            float bonusPercentiage = this.sceneBrain.getBonusPercentiege();





            //fake implementation

            //System.Diagnostics.Debug.WriteLine("************ numOfItem=" + maxNumberOfUserFriendlyGameObjectAllowed);

            Random random = new Random();
            for (int fakeIndex = 0; fakeIndex < maxNumberOfUserFriendlyGameObjectAllowed; fakeIndex++)
            {


                int xPosition = random.Next(0, 800);
                int yPosition = random.Next(0, 800);


                Geometry geometry = new EllipseGeometry(new Point(xPosition, yPosition), 150, 150);
                geometry.Freeze();

                ImageSource imageSource = new BitmapImage(new Uri(@"pack://application:,,,/Resources/skype.png"));
                imageSource.Freeze();

                this.sceneBrain.addGameObject(new UserFriendlyGameObject(geometry, imageSource, random.Next(200, 3000)));

            }

        }
    }
}
