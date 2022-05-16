using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KarenLogic
{

    public class SuperKaren
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public int Health { get; private set; }
        public bool IsPresent { get; private set; }

        public PictureBox pic;

        public SuperKaren(PictureBox pic)
        {
            this.pic = pic;
            this.pic.Visible = false;
            this.IsPresent = false;
            this.Health = 100;
        }

        public void Appear()
        {
            this.pic.Visible = true;
            this.IsPresent = true;
            this.pic.BringToFront();
        }

        public void Damage(int amount)
        {
            Health -= amount;
            if (Health < 0)
            {
                Game.AddToScore(5.95f);
                this.pic.Visible = false;
                this.IsPresent = false;
            }


        }
    }
}
