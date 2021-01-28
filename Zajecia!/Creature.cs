using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;


namespace Projekt
{

    public partial class Creature : UserControl
    {
        SimEngine sim;

        public static int size = 20;
        static float InitEnergy = 2000;
        static List<int> ang_vel_list = new List<int> { -2, 0, 2 };

        float x = 0;
        float y = 0;
        int ang = 0;

        public float Sense { get; private set; } = 1;
        public float Speed { get; private set; } = 1;
        public bool ReachHome { get; private set; } = false;

        float energy = InitEnergy;
        int ang_vel = 0;
        int FoodAngle = 0;
        int EatenFood = 0;
        bool FoodInsight = false;

        // konstruktor klonujący
        public Creature(Creature parent)
        {
            InitializeComponent();
            this.sim = parent.sim;
            Parent = parent.Parent;

            this.x = parent.x;
            this.y = parent.y;
            this.ang = parent.ang;
            this.Sense = parent.Sense;
            this.Speed = parent.Speed;

            Width = size;
            Height = size;

            this.Envolve();
            this.Step();

        }

        // konstruktor podstawowy
        public Creature(Panel parent, SimEngine sim, float x=0, float y = 0, int dir=0)
        {
            InitializeComponent();
            this.sim = sim;
            Parent = parent;

            this.x = x;
            this.y = y;
            this.ang = dir;

            Width = size;
            Height = size;

            this.Step();
        }


        void Envolve()
        {
            this.Sense += (5 - (float)SimEngine.random.Next(10)) / 12;
            this.Speed += (5 - (float)SimEngine.random.Next(10)) / 12;
        }

        public void Reset()
        {
            this.energy = InitEnergy;
            this.ang = (this.ang + 180) % 360;
            this.EatenFood = 0;
            this.ReachHome = false;
        }

        void FindFood()
        {
            List<Food> FoodToEat = new List<Food>();
            FoodInsight = false;
            float closest_food_dist = 10000;

            foreach (Food food in sim.Foods) // iteracja po liście dostępnego jedzenia
            {
                float dist = food.GetDist((int)this.x, (int)this.y);
                if (dist < Food.size/2 + size/2)
                {
                    FoodToEat.Add(food);
                } else if (dist < this.Sense * 50)
                {
                    if (dist < closest_food_dist)
                    {
                        closest_food_dist = dist;
                        this.FoodAngle = (food.getAngle((int)this.x, (int)this.y) + 180) % 360;
                        this.FoodInsight = true;

                    }
                }


            }
            foreach (Food food in FoodToEat)
            {
                sim.eatFood(food);
                this.EatenFood++;
            }
        }

        public void Step()
        {
            if (this.energy > 0 && !this.ReachHome)
            {
                if (this.EatenFood < 2) // szukamy jedzenia
                {

                    if (this.FoodInsight) // kierujemy się prosto do jedzenia
                    {
                        this.ang = this.FoodAngle;
                    }
                    else // losowe poruszanie się po mapie żeby coś znaleźć
                    {
                        if (SimEngine.random.Next(10) == 0) // 1:10 szansy na zmianę prędkości kątowej
                        {
                            int index = SimEngine.random.Next(ang_vel_list.Count); // losowanie nowej wartości prędkości kątowej z listy
                            this.ang_vel = ang_vel_list[index];
                        }
                        this.ang += this.ang_vel; // obrót o kąt 
                    }
                }
                else // najkrótsza droga do krawędzi
                {

                    bool is_over_d0 = (this.y < this.x * Parent.ClientRectangle.Height / Parent.ClientRectangle.Width);
                    bool is_over_d1 = (this.y < -this.x * Parent.ClientRectangle.Height / Parent.ClientRectangle.Width + Parent.ClientRectangle.Height);
                    if (is_over_d0)
                    {
                        if (is_over_d1)
                        {
                            this.ang = 270;
                        }
                        else
                        {
                            this.ang = 0;

                        }
                    }
                    else
                    {
                        if (is_over_d1)
                        {
                            this.ang = 180;
                        }
                        else
                        {
                            this.ang = 90;
                        }
                    }
                }


                // przemieszczenie się w osi x i y
                float new_x = this.x + (float)Math.Cos((Math.PI / 180) * this.ang) * this.Speed;
                float new_y = this.y + (float)Math.Sin((Math.PI / 180) * this.ang) * this.Speed;
                
                // wykrywanie krawędzi
                if ((new_x - size / 2) < 0 || 
                    (new_y - size / 2) < 0 || 
                    (new_x + size / 2) > Parent.ClientRectangle.Width ||
                    (new_y + size / 2) > Parent.ClientRectangle.Height
                    )
                {
                    if (this.EatenFood < 2)
                    {
                        this.ang = (this.ang + 180) % 360;
                    } else
                    {
                        this.ReachHome = true;
                    }
                }

                //aktualizacja pozycji
                this.x = new_x;
                this.y = new_y;
                Left = (int)(this.x - (size / 2));
                Top = (int)(this.y - (size / 2));


                // pobranie energii potrzebnej na dany ruch
                this.energy -= this.Speed * this.Speed + this.Sense;

                // poszukiwanie jedzenia
                this.FindFood();
            }


        }


        protected override void OnPaint(PaintEventArgs e)
        {
            // Call the OnPaint method of the base class.
            base.OnPaint(e);

            SolidBrush brush = new SolidBrush(Color.Blue);
            e.Graphics.FillEllipse(brush, 0, 0, (float)size-1, (float)size-1);
        }

    }
}
