using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Projekt
{
    public class SimEngine
    {
        public Panel mainPanel;
        public static Random random;

        public List<Food> Foods = new List<Food>();
        public List<Creature> CreatureList= new List<Creature>();

        public Timer steptimer;

        public int Epoh { get; private set; } = 0;

        public int FoodAmount { get; set; } = 30;

        int CreaturesAmount = 10;

        public SimEngine(Panel panel)
        {
            mainPanel = panel;
            random = new Random();

            // setup and bind Timer
            steptimer = new Timer();
            steptimer.Tick += StepTimerCallback;
            steptimer.Interval = 5;
            steptimer.Start();

        }

        public List<float[]> getPlotData()
        {
            List<float[]> dataToPlot = new List<float[]>();

            foreach (Creature creature in CreatureList)
            {
                dataToPlot.Add(new float[2] { creature.Speed, creature.Sense});
            }

            return dataToPlot;

        }


        public void NextEpoh()
        {
            if (this.Epoh == 0)
            {
                generateFood(FoodAmount);
                generateCreatures(CreaturesAmount);

            } else
            {
                celarAllFood();
                generateFood(FoodAmount);

                List<Creature> CreatureListCopy = new List<Creature>(CreatureList);

                foreach (Creature creature in CreatureListCopy)
                {
                    if (creature.ReachHome)
                    {
                        creature.Reset();
                        
                        Creature child1 = new Creature(creature);
                        Creature child2 = new Creature(creature);
                        CreatureList.Add(child1);
                        CreatureList.Add(child2);

                    }
                    mainPanel.Controls.Remove(creature);
                    CreatureList.Remove(creature);
                }
            }


            this.Epoh++;
        }


        private void StepTimerCallback(object sender, EventArgs e)
        {
            foreach (Creature creature in CreatureList)
            {
                creature.Step();
            }


        }

        public void removeCreatures()
        {
            foreach (Creature creature in CreatureList)
            {
                mainPanel.Controls.Remove(creature);
            }
            CreatureList.Clear();

        }


        public void generateCreatures(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                spawnCreatureOnEdge();
            }

        }

        public void spawnCreatureOnEdge()
        {
            int edge_n = random.Next(4);
            int x = 0;
            int y = 0;
            int dir = 0;
            switch (edge_n)
            {
                case 0:
                    x = Creature.size / 2;
                    y = Creature.size / 2 + random.Next(mainPanel.ClientRectangle.Height - Creature.size) ;
                    dir = 0;
                    break;
                case 1:
                    x = mainPanel.ClientRectangle.Width - Creature.size / 2;
                    y = Creature.size / 2 + random.Next(mainPanel.ClientRectangle.Height - Creature.size);
                    dir = 180;
                    break;
                case 2:
                    x = Creature.size / 2 + random.Next(mainPanel.ClientRectangle.Width - Creature.size);
                    y = Creature.size / 2;
                    dir = 90;
                    break;
                case 3:
                    x = Creature.size / 2 + random.Next(mainPanel.ClientRectangle.Width - Creature.size);
                    y = mainPanel.ClientRectangle.Height - Creature.size / 2;
                    dir = 270;
                    break;
            }
            this.spawnCreature(x, y, dir);

        }


        public void spawnCreature(int x, int y, int dir=0) {
            Creature creature = new Creature(mainPanel, this, x, y, dir);
            CreatureList.Add(creature);
        }

        public void celarAllFood()
        {
            foreach (Food test_food in Foods)
            {
                mainPanel.Controls.Remove(test_food);
            }
            Foods.Clear();

        }

        public void eatFood(Food food)
        {
            if (mainPanel.Controls.Contains(food)) mainPanel.Controls.Remove(food);
            if (Foods.Contains(food)) Foods.Remove(food);
        }


        public void generateFood(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                int new_x = 0;
                int new_y = 0;

                bool new_pos_found = false;

                while (!new_pos_found)
                {
                    new_pos_found = true;
                    new_x = Food.size + random.Next(mainPanel.ClientRectangle.Width - Food.size * 3);
                    new_y = Food.size + random.Next(mainPanel.ClientRectangle.Height - Food.size * 3);
                    foreach (Food test_food in Foods)
                    {
                        if (test_food.CheckColide(new_x, new_y, Food.size+2))
                        {
                            new_pos_found = false;
                        }
                    }

                }
                Food food = new Food(mainPanel, new_x, new_y);

                food.Parent = mainPanel;
                Foods.Add(food);
            }
        }




    }
}
