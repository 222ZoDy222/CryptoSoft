using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoSoftTest
{
    public class Match
    {
        /// <summary>
        /// Начальная точка
        /// </summary>
        public Coord x1;

        /// <summary>
        /// Конечная точка
        /// </summary>
        public Coord x2;

        /// <summary>
        /// Время сгорания
        /// </summary>
        public uint time;

        public bool[] fireLine;

        /// <summary>
        /// Это спичка с длиной корень из 2?
        /// </summary>
        public bool isBigMatch = false;

        public bool burned 
        {
        
            get
            {
                for (int i = 0; i < fireLine.Length; i++)
                {
                    if (!fireLine[i]) return false;
                }
                return true;
            }

        }

        public CoordFloat centerSquare()
        {
            
            if (this.x1.x == this.x2.x || this.x1.y == this.x2.y) return null;
            CoordFloat result = new CoordFloat();
            result.x = (this.x1.x + this.x2.x) / 2f;
            result.y = (this.x1.y + this.x2.y) / 2f;
            return result;
        }

        /// <summary>
        /// По Первой координате есть огонь?
        /// </summary>
        public bool isX1OnFire
        {
            get => fireLine[0];
        }
        /// <summary>
        /// Подожглась передняя часть в этом кадре?
        /// </summary>
        public bool FrameX1Rate = false;

        /// <summary>
        /// По второй координате есть огонь?
        /// </summary>
        public bool isX2OnFire
        {
            get => fireLine[fireLine.Length - 1];
        }
        /// <summary>
        /// Подожглась задняя часть в этом кадре?
        /// </summary>
        public bool FrameX2Rate = false;

        /// <summary>
        /// Развивался ли огонь в этом кадре на этой спичке
        /// </summary>
        public bool FrameHaveFire = false;

        public bool isCenterOnFire
        {
            get
            {
                // Если например спичка из 6 частей горения
                // То проверяем 2 части --++-- = по 2 и 3 индексу
                if (fireLine.Length % 2 == 0)
                {
                    int burnedIndex = fireLine.Length / 2; // Если 6 частей, то вернет 3
                    if (fireLine[burnedIndex]) return true;                    
                    if (burnedIndex > 0)
                        if (fireLine[burnedIndex-1]) return true;
                }
                else // Если 5 например --+--
                {
                    if (fireLine[fireLine.Length / 2]) return true;
                }
                return false;
            }
        }
        /// <summary>
        /// Поджегся центр в этом кадре?
        /// </summary>
        public bool FrameCenterRate = false;


        public Match(int x1, int y1, int x2, int y2, uint time) 
        {
            this.x1 = new Coord(x1, y1);
            this.x2 = new Coord(x2, y2);
            this.time = time;
            fireLine = new bool[time * 25];
            if (centerSquare() != null) isBigMatch = true;
        }

        public void Fire()
        {
            FrameX1Rate = false;
            FrameX2Rate = false;
            FrameCenterRate = false;
            FrameHaveFire = false;
            bool isCenterWasInFire = isCenterOnFire;

            for (int i = 0; i < fireLine.Length; i++)
            {
                // Если это первая часть
                if (i == 0)
                {
                    if (fireLine[i] == true)
                    {

                        if (fireLine.Length == 1) break;
                        // Если соседи тоже в огне
                        if (fireLine[i + 1]) continue;

                        fireLine[i + 1] = true;
                        FrameHaveFire = true;
                        if (i != fireLine.Length - 1) i++;
                    }

                }
                else if (i < fireLine.Length-1)
                {
                    // Если часть в огне
                    if(fireLine[i] == true)
                    {
                        // Если соседи тоже в огне
                        if (fireLine[i - 1] && fireLine[i + 1]) continue;
                        else 
                        {
                            // В этом кадре подожглась передняя часть
                            if (i == 1 && !fireLine[0])
                            {
                                FrameX1Rate = true;
                            }
                            // В этом кадре подожглась задняя часть Если это предпоследний индекс и последний еще не был подожжен
                            else if (i == fireLine.Length - 2 && !fireLine[fireLine.Length-1])
                            {
                                FrameX2Rate = true;
                            }

                            //FrameCenterRate
                            // Если центр еще не подожжен
                            if (!isCenterWasInFire)
                            {
                                // Если например спичка из 6 частей горения
                                // То проверяем 2 части --++-- = по 2 и 3 индексу
                                if (fireLine.Length % 2 == 0)
                                {
                                    // и мы сейчас находимся в центре и его нужно поджечь
                                    if(i == (fireLine.Length / 2) - 1 || i == (fireLine.Length / 2) - 2)
                                    {
                                        FrameCenterRate = true;
                                    }
                                }
                                else // Если 5 например --+--
                                {
                                    // и мы сейчас находимся в центре и его нужно поджечь
                                    if (i == (fireLine.Length / 2) - 1)
                                    {
                                        FrameCenterRate = true;
                                    }
                                }
                            }
                            

                            fireLine[i - 1] = true;
                            fireLine[i + 1] = true;
                            FrameHaveFire = true;
                            if (i != fireLine.Length - 1) i++;

                        }
                    }
                }  
                else// Если это последняя часть
                {
                    // Если на ней огонь
                    if(fireLine[i] == true)
                    {
                        if(i != 0)
                        {
                            if (!fireLine[i - 1])
                            {
                                fireLine[i - 1] = true;
                                FrameHaveFire = true;
                            }
                        }
                        
                        
                    }
                }

            }
        }







        /// <summary>
        /// Поджечь спичку на координате x y, если нет такой координаты, то ничего не происходит
        /// </summary>
        public void FireOn(int x, int y)
        {
            // Если поджигается передняя часть
            if(x1.x == x && x1.y == y)
            {
                fireLine[0] = true;
                
            } else if(x2.x == x && x2.y == y)
            {
                // Если поджигается задняя часть
                fireLine[fireLine.Length-1] = true;
                
            }

        }

        public void FireOn(float x, float y)
        {
            // Если спичка маленькая, то ей нельзя будет поджечь центр
            if (!isBigMatch) return;

            CoordFloat coordsFloat = centerSquare();
            // Если координаты центра совпадают, то поджигаем центр спички
            if(x == coordsFloat.x && y == coordsFloat.y)
            {
                // Если число делится на 2 без остатка
                // --++-- (+ это то что нужно поджечь)                
                if (fireLine.Length % 2 == 0)
                {
                    int indexToBurn = fireLine.Length / 2;
                    fireLine[indexToBurn] = true;
                   
                    if (indexToBurn > 0)
                        fireLine[indexToBurn - 1] = true;
                } 
                else // Если 5 например --+--
                {
                    fireLine[fireLine.Length / 2] = true;
                    
                }
                //fireLine[fireLine.Length/2]
            }

        }


        public Match GetClone()
        {
            return new Match(x1.x, x1.y, x2.x, x2.y, time);
        }

    }

    

    /// <summary>
    /// Координаты спичек
    /// </summary>
    public class Coord 
    {
        public int x;
        public int y;

        public Coord() { }
        
        public Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Coord GetClone()
        {
            return new Coord(x, y);
        }
    }

    public class CoordFloat 
    {
        public float x;
        public float y;
    }

}
