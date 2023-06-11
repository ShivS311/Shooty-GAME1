using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shooty_Game
{
    public partial class Form1 : Form
    {

        bool goLeft, goRight, goUp, goDown, gameOver;
        string facing = "up";
        int playerHealth = 200;
        int speed = 10;
        int ammo = 10;
        int score;
        int zombieSpeed = 3;
        Random randNum = new Random();

        List<PictureBox> zombiesList = new List<PictureBox>();








        public Form1()
        {
            InitializeComponent();
            RestartGame();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void gameHealth_Click(object sender, EventArgs e)
        {

        }

        private void MainTimerEvent(object sender, EventArgs e)
        {
            if (playerHealth > healthBar.Maximum)
            {
                playerHealth = healthBar.Maximum;
            }
            else if (playerHealth < healthBar.Minimum)
            {
                playerHealth = healthBar.Minimum;
            }

            healthBar.Value = playerHealth;

            if (playerHealth <= 0)
            {
                gameOver = true;
                player.Image = Properties.Resources.dead;
                GameTimer.Stop();
            }

            txtAmmo.Text = "Ammo: " + ammo;
            txtKills.Text = "Kills: " + score;

            if (goLeft && player.Left > 0)
            {
                player.Left -= speed;
            }
            if (goRight && player.Left + player.Width < ClientSize.Width)
            {
                player.Left += speed;
            }
            if (goUp && player.Top > 45)
            {
                player.Top -= speed;
            }
            if (goDown && player.Top + player.Height < ClientSize.Height)
            {
                player.Top += speed;
            }

            foreach (Control x in Controls)
            {
                if (x is PictureBox && x.Tag == "ammo")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        Controls.Remove(x);
                        ((PictureBox)x).Dispose();
                        ammo += 5;
                        playerHealth += 20;
                    }
                }

                if (x is PictureBox && x.Tag == "zombie")
                {
                    PictureBox zombie = (PictureBox)x;

                    if (player.Bounds.IntersectsWith(zombie.Bounds))
                    {
                        playerHealth -= 1;
                    }

                    if (zombie.Left > player.Left)
                    {
                        zombie.Left -= zombieSpeed;
                        zombie.Image = Properties.Resources.zleft;
                    }
                    if (zombie.Left < player.Left)
                    {
                        zombie.Left += zombieSpeed;
                        zombie.Image = Properties.Resources.zright;
                    }
                    if (zombie.Top > player.Top)
                    {
                        zombie.Top -= zombieSpeed;
                        zombie.Image = Properties.Resources.zup;
                    }
                    if (zombie.Top < player.Top)
                    {
                        zombie.Top += zombieSpeed;
                        zombie.Image = Properties.Resources.zdown;
                    }

                    foreach (Control j in Controls)
                    {
                        if (j is PictureBox && j.Tag == "bullet")
                        {
                            PictureBox bullet = (PictureBox)j;

                            if (zombie.Bounds.IntersectsWith(bullet.Bounds))
                            {
                                score++;

                                Controls.Remove(bullet);
                                bullet.Dispose();
                                Controls.Remove(zombie);
                                zombie.Dispose();
                                zombiesList.Remove(zombie);
                                MakeZombies();
                                break; // Exit the inner loop since the bullet hit a zombie
                            }
                        }
                    }
                }
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (gameOver == true)
            {
                return;
            }








            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
                facing = "left";
                player.Image = Properties.Resources.left;
            }
            else if (e.KeyCode == Keys.Right)
            {
                goRight = true;
                facing = "right";
                player.Image = Properties.Resources.right;
            }
            else if (e.KeyCode == Keys.Up)
            {
                goUp = true;
                facing = "up";
                player.Image = Properties.Resources.up;
            }
            else if (e.KeyCode == Keys.Down)
            {
                goDown = true;
                facing = "down";
                player.Image = Properties.Resources.down;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            else if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            else if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
            else if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }

            if (e.KeyCode == Keys.Space && ammo > 0 && gameOver == false)
            {
                ammo--;
                ShootBullet(facing);

                if (ammo < 1)
                {
                    DropAmmo();
                }
            }

            if (e.KeyCode == Keys.Enter && gameOver == true)
            {
                RestartGame();
            }







        }



        private void ShootBullet(string direction)
        {
            Bullet shootBullet = new Bullet(this);
            shootBullet.direction = direction;
            shootBullet.bulletLeft = player.Left + (player.Width / 2);
            shootBullet.bulletTop = player.Top + (player.Height / 2);
            shootBullet.MakeBullet();
        }


        private void MakeZombies()
        {
            PictureBox zombie = new PictureBox();
            zombie.Tag = "zombie";
            zombie.Image = Properties.Resources.zdown;
            zombie.Left = randNum.Next(0, 900);
            zombie.Top = randNum.Next(0, 800);
            zombie.SizeMode = PictureBoxSizeMode.AutoSize;
            zombiesList.Add(zombie);
            this.Controls.Add(zombie);
            player.BringToFront();






        }


        private void DropAmmo()
        {
            PictureBox ammo = new PictureBox();
            ammo.Image = Properties.Resources.ammo_Image;
            ammo.SizeMode = PictureBoxSizeMode.AutoSize;
            ammo.Left = randNum.Next(10, this.ClientSize.Width - ammo.Width);
            ammo.Top = randNum.Next(60, this.ClientSize.Height - ammo.Height);
            ammo.Tag = "ammo";
            this.Controls.Add(ammo);

            ammo.BringToFront();
            player.BringToFront();








        }





        private void RestartGame()
        {
            player.Image = Properties.Resources.up;

            foreach (PictureBox i in zombiesList)
            {
                Controls.Remove(i);
            }

            zombiesList.Clear();

            // Remove ammo boxes
            foreach (Control control in Controls.OfType<PictureBox>().ToList())
            {
                if ((string)control.Tag == "ammo")
                {
                    Controls.Remove(control);
                    control.Dispose();
                }
            }

            for (int i = 0; i < 3; i++)
            {
                MakeZombies();
            }

            goUp = false;
            goDown = false;
            goLeft = false;
            goRight = false;
            gameOver = false;
            playerHealth = 200;
            score = 0;
            ammo = 10;

            GameTimer.Start();
        }

    }
}



