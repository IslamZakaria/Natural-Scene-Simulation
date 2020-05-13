using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace Graphics
{
    class Ground
    {
        Model groundModel;
        public float xMax = -100000000;
        public float xMin = 100000000;
        public float yMax = -100000000;
        public float yMin = 100000000;
        public float zMax = -100000000;
        public float zMin = 100000000;
        public float maxTerrianHight = -100000000;
        public Bitmap hightMap;
        public float[,] heights;
        public List<mat4> Transformations = new List<mat4>();
        public Ground(string projectPath)
        {
            groundModel = new Model();

            //getting hightMap of Ground
            hightMap = (Bitmap)Bitmap.FromFile(projectPath + "\\Textures\\heightmap.jpg");
            heights = new float[hightMap.Width, hightMap.Height];
            for (int x = 0; x < hightMap.Width ; x++)
            {
                for (int y = 0; y < hightMap.Height ; y++)
                {
                    Color Pixel = hightMap.GetPixel(x, y);
                    heights[x, y] = Pixel.R;
                    maxTerrianHight = Math.Max(heights[x, y], maxTerrianHight);
                }
            }
            maxTerrianHight = maxTerrianHight * (float)0.3;
            maxTerrianHight = maxTerrianHight - 50;

            //for each point i'll check right(i+1,j) ,down right(i+1,j+1) ,down(i,j+1) and point itSelf(i,j)
            for (int i = 0; i < hightMap.Width - 1; i++)
            {
                for (int j = 0; j < hightMap.Height - 1; j++)
                {
                    /*[0,0]     [1,0]
                     * (3-6)____(4)
                     * |          |  
                     * |          |
                     * (1)_______(2-5)
                     * [0,1]      [1,1]
                     */

                    //v1-->(i,j+1)
                    vec3 v1 = new vec3(i,heights[i, j + 1],j+1);//i FOR x  --  heights list is FOR y  --  j FOR z
                    xMax = Math.Max(i, xMax);
                    xMin = Math.Min(i, xMin);
                    yMax = Math.Max(heights[i, j + 1], yMax);
                    yMin = Math.Min(heights[i, j + 1], yMin);
                    zMax = Math.Max(j + 1, zMax);
                    zMin = Math.Min(j + 1, zMin);
                    //v2-->(i+1,j+1)
                    vec3 v2 = new vec3(i + 1, heights[i + 1, j + 1], j + 1);//i FOR x  --  heights list is FOR y  --  j FOR z
                    xMax = Math.Max(i+1, xMax);
                    xMin = Math.Min(i + 1, xMin);
                    yMax = Math.Max(heights[i + 1, j + 1], yMax);
                    yMin = Math.Min(heights[i + 1, j + 1], yMin);
                    zMax = Math.Max(j + 1, zMax);
                    zMin = Math.Min(j + 1, zMin);
                    //v3-->(i,j)
                    vec3 v3 = new vec3(i, heights[i, j], j);//i FOR x  --  heights list is FOR y  --  j FOR z
                    xMax = Math.Max(i, xMax);
                    xMin = Math.Min(i, xMin);
                    yMax = Math.Max(heights[i, j], yMax);
                    yMin = Math.Min(heights[i, j], yMin);
                    zMax = Math.Max(j , zMax);
                    zMin = Math.Min(j , zMin);
                    //v4-->(i+1,j)
                    vec3 v4 = new vec3(i + 1, heights[i + 1, j], j);//i FOR x  --  heights list is FOR y  --  j FOR z
                    xMax = Math.Max(i+1, xMax);
                    xMin = Math.Min(i+1, xMin);
                    yMax = Math.Max(heights[i + 1, j], yMax);
                    yMin = Math.Min(heights[i + 1, j], yMin);
                    zMax = Math.Max(j , zMax);
                    zMin = Math.Min(j , zMin);
                    //v5--> is equals to v2(i+1,j+1)
                    vec3 v5 = new vec3(i + 1, heights[i + 1, j + 1], j + 1);//i FOR x  --  heights list is FOR y  --  j FOR z
                    xMax = Math.Max(i + 1, xMax);
                    xMin = Math.Min(i + 1, xMin);
                    yMax = Math.Max(heights[i + 1, j + 1], yMax);
                    yMin = Math.Min(heights[i + 1, j + 1], yMin);
                    zMax = Math.Max(j + 1, zMax);
                    zMin = Math.Min(j + 1, zMin);
                    //v6--> is equals to v3(i,j)
                    vec3 v6 = new vec3(i, heights[i, j], j);//i FOR x  --  heights list is FOR y  --  j FOR z
                    xMax = Math.Max(i, xMax);
                    xMin = Math.Min(i, xMin);
                    yMax = Math.Max(heights[i, j + 1], yMax);
                    yMin = Math.Min(heights[i, j + 1], yMin);
                    zMax = Math.Max(j + 1, zMax);
                    zMin = Math.Min(j + 1, zMin);
                    //ADDING UV DATA FOR V1
                    vec2 uv1 = new vec2((float)i / (16), ((float)j + 1) / (20));
                    //ADDING UV DATA FOR V2
                    vec2 uv2 = new vec2((float)(i + 1) / (16), (float)(j + 1) / (20));
                    //ADDING UV DATA FOR V3               16      1024           20                 1024
                    vec2 uv3 = new vec2((float)(0 + i) / (16), (float)(0 + j) / (20));
                    //ADDING UV DATA FOR V4               16   (float)   1024    20                        1024
                    vec2 uv4 = new vec2((float)(1 + i) / (16), (float)(0 + j) / (20));
                    //ADDING UV DATA FOR V5
                    vec2 uv5 = uv2;
                    //ADDING UV DATA FOR V6
                    vec2 uv6 = uv3;
                    //getting normals
                    vec3 vectorThree = v3 - v6;
                    vec3 vectorTwo = v2 - v6;
                    vec3 normalOfFirstTriangle = glm.cross(vectorThree, vectorTwo);//rightHand--->(UP)
                    vec3 vectorSix = v6 - v4;
                    vec3 vectorFive = v5 - v4;
                    vec3 normalOfSecondTriangle = glm.cross(vectorSix, vectorFive);//rightHand--->(UP)
                    //add vertex position, norma, uv in the ground model
                    groundModel.vertices.Add(v1);
                    groundModel.vertices.Add(v2);
                    groundModel.vertices.Add(v3);
                    groundModel.vertices.Add(v4);
                    groundModel.vertices.Add(v5);
                    groundModel.vertices.Add(v6);

                    groundModel.uvCoordinates.Add(uv1);
                    groundModel.uvCoordinates.Add(uv2);
                    groundModel.uvCoordinates.Add(uv3);
                    groundModel.uvCoordinates.Add(uv4);
                    groundModel.uvCoordinates.Add(uv5);
                    groundModel.uvCoordinates.Add(uv6);

                    groundModel.normals.Add(normalOfFirstTriangle);
                    groundModel.normals.Add(normalOfFirstTriangle);
                    groundModel.normals.Add(normalOfFirstTriangle);
                    groundModel.normals.Add(normalOfSecondTriangle);
                    groundModel.normals.Add(normalOfSecondTriangle);
                    groundModel.normals.Add(normalOfSecondTriangle);
                }
            }
            Transformations.Add(glm.scale(new mat4(1), new vec3(0.195f, 0.3f, 0.195f)));//0.195f, 0.3f, 0.195f
            Transformations.Add(glm.translate(new mat4(2), new vec3(-50.0f, -50.0f, -50.0f)));
            xMax = xMax * (float)0.195;
            xMax = xMax - 50;
            xMin = xMin * (float)0.195;
            xMin = xMin - 50;
            yMax = yMax * (float)0.3;
            yMax = yMax - 50;
            yMin = yMin * (float)0.3;
            yMin = yMin - 50;
            zMax = zMax * (float)0.195;
            zMax = zMax - 50;
            zMin = zMin * (float)0.195;
            zMin = zMin - 50;
            groundModel.transformationMatrix = MathHelper.MultiplyMatrices(Transformations);
            groundModel.Initialize();
        }
        public Ground(float width, float length, float height, int stride)
        {
            groundModel = new Model();
            Random r = new Random();

            float[,] heights = new float[(int)(width / stride)+1, (int)(length / stride)+1];

            for (int i = 0; i < width / stride; i++)
            {
                for (int j = 0; j < length / stride; j++)
                {
                    
                    heights[i, j] = (float)r.NextDouble() * height;
                }
            }
            for (int i = 0; i < width - stride; i += stride)
            {
                for (int j = 0; j < length - stride; j += stride)
                {
                    //add vertex position, norma, uv in the ground model
                    vec3 v1 = new vec3(i - width / 2, heights[(int)(i / stride), (int)(j / stride)], j - length / 2);
                    vec3 v2 = new vec3(i - width / 2 + stride, heights[(int)(i / stride) + 1, (int)(j / stride)], j - length / 2);
                    vec3 v3 = new vec3(i - width / 2, heights[(int)(i / stride), (int)(j / stride) + 1], j - length / 2 + stride);

                    groundModel.vertices.Add(v1);
                    groundModel.vertices.Add(v2);
                    groundModel.vertices.Add(v3);

                    vec3 n1 = v3 - v1;
                    vec3 n2 = v2 - v1;
                    vec3 t1Normal = glm.cross(n1, n2);
                    t1Normal = glm.normalize(t1Normal);

                    groundModel.normals.Add(t1Normal);
                    groundModel.normals.Add(t1Normal);
                    groundModel.normals.Add(t1Normal);

                    vec3 v4 = new vec3(i - width / 2 + stride, heights[(int)(i / stride) + 1, (int)(j / stride)], j - length / 2);
                    vec3 v5 = new vec3(i - width / 2, heights[(int)(i / stride), (int)(j / stride) + 1], j - length / 2 + stride);
                    vec3 v6 = new vec3(i - width / 2 + stride, heights[(int)(i / stride) + 1, (int)(j / stride) + 1], j - length / 2 + stride);

                    groundModel.vertices.Add(v6);
                    groundModel.vertices.Add(v5);
                    groundModel.vertices.Add(v4);

                    n1 = v4 - v6;
                    n2 = v5 - v6;


                    t1Normal = glm.cross(n1, n2);
                    t1Normal = glm.normalize(t1Normal);

                    groundModel.normals.Add(t1Normal);
                    groundModel.normals.Add(t1Normal);
                    groundModel.normals.Add(t1Normal);

                    groundModel.uvCoordinates.Add(new vec2(0, 0));
                    groundModel.uvCoordinates.Add(new vec2(1, 0));
                    groundModel.uvCoordinates.Add(new vec2(0, 1));
                    groundModel.uvCoordinates.Add(new vec2(1, 0));
                    groundModel.uvCoordinates.Add(new vec2(0, 1));
                    groundModel.uvCoordinates.Add(new vec2(1, 1));
                }
            }
            Transformations.Add(glm.scale(new mat4(1), new vec3(0.2f, 0.3f, 0.18f)));//0.195f, 0.3f, 0.195f
            Transformations.Add(glm.translate(new mat4(2), new vec3(-10.0f, -30.0f, 4.0f)));
            groundModel.transformationMatrix = MathHelper.MultiplyMatrices(Transformations);
            groundModel.Initialize();
        }
        public void Draw(int matID)
        {
            groundModel.Draw(matID);
            

        }
    }
}
