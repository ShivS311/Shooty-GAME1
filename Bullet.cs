using System;
using System.Drawing;
using System.Windows.Forms;

namespace Shooty_Game
{
    internal class Bullet
    {
        public string direction;
        public int bulletLeft;
        public int bulletTop;

        private int speed = 20;
        private PictureBox bullet = new PictureBox();
        private Timer bulletTimer = new Timer();
        private Form parentForm;

        public Bullet(Form form)
        {
            parentForm = form;
        }

        public void MakeBullet()
        {
            bullet.BackColor = Color.White;
            bullet.Size = new Size(5, 5);
            bullet.Tag = "bullet";
            bullet.Left = bulletLeft;
            bullet.Top = bulletTop;
            bullet.BringToFront();

            parentForm.Controls.Add(bullet);

            bulletTimer.Interval = speed;
            bulletTimer.Tick += BulletTimerEvent;
            bulletTimer.Start();
        }

        private void BulletTimerEvent(object sender, EventArgs e)
        {
            if (direction == "left")
            {
                bullet.Left -= speed;
            }
            else if (direction == "right")
            {
                bullet.Left += speed;
            }
            else if (direction == "up")
            {
                bullet.Top -= speed;
            }
            else if (direction == "down")
            {
                bullet.Top += speed;
            }

            if (bullet.Left < 10 || bullet.Left > 860 || bullet.Top < 10 || bullet.Top > 600)
            {
                bulletTimer.Stop();
                bulletTimer.Dispose();
                bulletTimer = null;

                if (bullet != null)
                {
                    parentForm.Controls.Remove(bullet);
                    bullet.Dispose();
                    bullet = null;
                }
            }
        }
    }
}
