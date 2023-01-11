using System;
using System.Collections.Generic;
using System.Text;
using CryptoSoftTest.ClassContainer;

namespace CryptoSoftTest
{
    
    public class FireCalculator
    {

        /// <summary>
        /// Высчитывание наилучшего варианта
        /// </summary>
        /// <param name="plane">Поле со всеми спичками</param>
        /// <returns> Координаты наилучшего варианта поджигания и сколько фреймов ушло</returns>
        public int[] Calculate(CoordinatePlane plane)
        {
            // Координаты, которые уже были проверены
            List<Coord> coordsHaveChecked = new List<Coord>();

            int bestResult = int.MaxValue;
            Coord bestCoords = new Coord();

            for (int i = 0; i < plane.matches.Count; i++)
            {
                CoordinatePlane newPlane = plane.GetCopy();
                int result = CalculateVariant(newPlane, newPlane.matches[i], true);
                if(result == -1)
                {
                    return new int[3] { 0, 0, -1 };
                }
                if(result <= bestResult)
                {
                    bestResult = result;
                    bestCoords = plane.matches[i].x1;
                }

                newPlane = plane.GetCopy();
                result = CalculateVariant(newPlane, newPlane.matches[i], false);
                if (result <= bestResult)
                {
                    bestResult = result;
                    bestCoords = plane.matches[i].x2;
                }
            }

            int[] calculateResult = new int[3];
            calculateResult[0] = bestCoords.x;
            calculateResult[1] = bestCoords.y;
            calculateResult[2] = bestResult;
            return calculateResult;
        }

       

        public int CalculateVariant(CoordinatePlane plane, Match match, bool frontSide)
        {
            if (frontSide) plane.FireOn(match.x1.x, match.x1.y);
            else plane.FireOn(match.x2.x, match.x2.y);
            if(match.x1.x == 1 && match.x1.y == -1)
            {
                int t = 0;
            }
            int frameCount = 1;

            int frameFireCounter = 0;
            
            //Пока все спички не сгорят
            while (!plane.IsAllBurned)
            {
                plane.Fire();
                frameCount++;
                


                // Если огонь перестал разгораться по спичкам, то видимо одна из спичек не соприкасается с другими
                if (!plane.FrameFireMove)
                {
                    frameFireCounter++;
                    if (frameFireCounter > 3)
                        return -1;
                }
                else frameFireCounter = 0;

            }

            return frameCount;
        }



    }
}
