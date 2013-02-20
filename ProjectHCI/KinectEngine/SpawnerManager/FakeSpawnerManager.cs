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


        public FakeSpawnerManager(ISceneBrain sceneBrain)
        {
            this.sceneBrain = sceneBrain;
        }


        public void awaken()
        {

            int maxNumberOfChopAllowed = this.sceneBrain.getMaxNumberOfChopAllowed();
            int maxNumberOfUserFriendlyGameObjectAllowed = this.sceneBrain.getMaxNumberOfUserFriendlyGameObjectAllowed();
            float bonusPercentiage = this.sceneBrain.getBonusPercentiege();





            //fake implementation

            

            for (int fakeIndex = 0; fakeIndex <= maxNumberOfUserFriendlyGameObjectAllowed; fakeIndex++)
            {


                Random random = new Random();
                int xPosition = random.Next(0, 1000);
                int yPosition = random.Next(0, 1000);


                Geometry geometry = new EllipseGeometry(new Point(xPosition, yPosition), 10, 10);
                geometry.Freeze();

                ImageSource imageSource = new BitmapImage(new Uri(@"pack://application:,,,/Resources/skype.png"));
                imageSource.Freeze();



                this.sceneBrain.addGameObject(new UserFriendlyGameObject(geometry, imageSource, 5000, null));

            }

        }
    }
}
