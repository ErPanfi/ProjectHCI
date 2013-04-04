using ProjectHCI.KinectEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace ProjectHCI.ReverseFruitNinja
{
    public class SoundGameObject : GameObject
    {


        private MediaPlayer mediaPlayer;
        private Uri fileNameUri;
        private bool loop;



        public SoundGameObject(Uri fileNameUri,
                               double volume,
                               bool loop)
            : this (fileNameUri, loop)
        {
            Debug.Assert(this.mediaPlayer != null, "this.mediaPlayer != null");
            this.mediaPlayer.Volume = volume;
        }




        public SoundGameObject(Uri fileNameUri,
                               bool loop)
        {
            base._xPosition = 0;
            base._yPosition = 0;
            base._gameObjectTag = Tags.SOUND_TAG;

            this.mediaPlayer = new MediaPlayer();

            this.fileNameUri = fileNameUri;
            this.loop = loop;

        }

        public override void update(int deltaTimeMillis)
        {
            //do nothing
        }

        public override bool isCollidable()
        {
            return false;
        }

        public override bool isDead()
        {
            if (this.loop)
            {
                return false;
            }
            else
            {
                return this.mediaPlayer.HasAudio && this.mediaPlayer.Position >= this.mediaPlayer.NaturalDuration.TimeSpan; 
            }

        }

        public override void onRendererDisplayDelegate()
        {
            this.mediaPlayer.Open(this.fileNameUri);
            this.mediaPlayer.Play();
        }

        public override void onRendererUpdateDelegate()
        {
            
            if (this.loop 
                && this.mediaPlayer.HasAudio
                && this.mediaPlayer.Position >= this.mediaPlayer.NaturalDuration.TimeSpan)
            {
                this.mediaPlayer.Stop();
                this.mediaPlayer.Position = TimeSpan.Zero;
                this.mediaPlayer.Play();
            }
        }

        public override void onRendererRemoveDelegate()
        {
            this.mediaPlayer.Stop();
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
