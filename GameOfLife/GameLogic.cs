using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    class GameLogic
    {
        private static readonly int num = Globals.igNum;

        public bool[,] Cells { get; set; } = new bool[num, num];

        public bool[,] newCells = new bool[num, num];

        public GameLogic(bool[,] cells)
        {
            Cells = cells;
        }

        public void update()
        {
            for(int i = 0; i < num; i++)
            {
                for(int j = 0; j < num; j++)
                {
                    if(Cells[i,j] == false)
                    {
                        int numOfNeighbors = numOfLife(Cells, i, j);
                        if (numOfNeighbors == 3) newCells[i, j] = true;
                        else newCells[i, j] = false;
                    }else
                    {
                        int numOfNeighbors = numOfLife(Cells, i, j);
                        if (numOfNeighbors == 2 || numOfNeighbors == 3) newCells[i, j] = true;
                        else newCells[i, j] = false;
                    }
                }
            }
            Cells = newCells;
        }

        private int numOfLife(bool[,] Cells, int i, int j)
        {
            int numOfNeighbors = 0;
            if (i != 0)
            {
                if (j != 0)
                {
                    if (Cells[i - 1, j - 1] == true) numOfNeighbors++;
                }
                if (Cells[i - 1, j] == true) numOfNeighbors++;
                if (j != num - 1)
                {
                    if (Cells[i - 1, j + 1] == true) numOfNeighbors++;
                }
            }
            if (j != 0)
            {
                if (Cells[i, j - 1] == true) numOfNeighbors++;
                if (i != num - 1)
                {
                    if (Cells[i + 1, j - 1] == true) numOfNeighbors++;
                }
            }
            if (j != num - 1)
            {
                if (Cells[i, j + 1] == true) numOfNeighbors++;
            }
            if (i != num - 1)
            {
                if (Cells[i + 1, j] == true) numOfNeighbors++;
                if (j != num - 1)
                {
                    if (Cells[i + 1, j + 1] == true) numOfNeighbors++;
                }
            }
            return numOfNeighbors;
        }

    }
}
