using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;
using GlmNet;
using System.IO;
using System.Drawing;
using Graphics._3D_Models;

namespace Graphics
{
    class Renderer
    {
        Shader sh;
        Shader shG;
        
        uint vertexBufferID;
        //uint vertexBufferIdGround;
        int transID;
        int viewID;
        int projID;

        int transIDG;
        int viewIDG;
        int projIDG;
        int groundIce;
        int groundSand;
        int groundGrass;
        int groundRock;
        int groundWater;
        mat4 scaleMat;
        //mat4 model;
        //int MVPID;
        //mat4 ModelMatrix;
        //mat4 MVP;

        int EyePositionID;
        int AmbientLightID;
        int DataID;
        vec3 Day;
        vec3 Night;
        float time = 0;

        mat4 ProjectionMatrix;
        mat4 ViewMatrix;

        public Camera cam;

        Texture front;
        Texture back;
        Texture left;
        Texture right;
        Texture up;
        Texture down;
        public Ground grond;
        Texture ice;
        Texture rock;
        Texture sand;
        Texture grass;
        vec3 ambientLight;
        Model3D tree;
        Random rand=new Random();
        int xAxis ;
        int zAxis ;
        List<float> xArr = new List<float>();
        List<float> zArr = new List<float>();
        
        Texture water;
        bool reverse;
        float t = 0;
        int timeID;
        Ground waterG;
        int controlDrawn;


        public void Initialize()
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");
            shG = new Shader(projectPath + "\\Shaders\\groundVert.vertexshader", projectPath + "\\Shaders\\groundFrag.fragmentshader");
        
            up = new Texture(projectPath + "\\Textures\\Up.jpg", Gl.GL_TEXTURE0, false);
            ice = new Texture(projectPath + "\\Textures\\Ice.jpg", Gl.GL_TEXTURE0, false);
            rock = new Texture(projectPath + "\\Textures\\Rock.jpg", Gl.GL_TEXTURE0, false);
            sand = new Texture(projectPath + "\\Textures\\Down.jpg", Gl.GL_TEXTURE0, false);
            grass = new Texture(projectPath + "\\Textures\\Grass.jpg", Gl.GL_TEXTURE0, false);
            left = new Texture(projectPath + "\\Textures\\Left.jpg", Gl.GL_TEXTURE0, false);
            right = new Texture(projectPath + "\\Textures\\Right.jpg", Gl.GL_TEXTURE0, false);
            front = new Texture(projectPath + "\\Textures\\Front.jpg", Gl.GL_TEXTURE0, false);
            back = new Texture(projectPath + "\\Textures\\Back.jpg", Gl.GL_TEXTURE0, false);
            down = new Texture(projectPath + "\\Textures\\Down1.jpg", Gl.GL_TEXTURE0, false);
            water = new Texture(projectPath + "\\Textures\\water.PNG", Gl.GL_TEXTURE0, false);
            Gl.glClearColor(1, 1, 1, 1);

            

            grond = new Ground(projectPath);
            float[] verts =
            {
                //FRONT
                -1.0f, -1.0f, 1.0f,
                0, 0, 1,
                0 , 1,
                1.0f, -1.0f, 1.0f,
                0, 0, 1,
                1 , 1,
                1.0f, 1.0f, 1.0f,
                0, 0, 1,
                1 , 0,
                -1.0f, -1.0f, 1.0f,
                0, 0, 1,
                0 , 1,
                1.0f, 1.0f, 1.0f,
                0, 0, 1,
                1 , 0,
                -1.0f, 1.0f, 1.0f,
                0, 0, 1,
                0 , 0,
                //BACK
                -1.0f, -1.0f, -1.0f,
                0, 0, 1,
                1 , 1,
                1.0f, -1.0f, -1.0f,
                0, 0, 1,
                0 , 1,
                1.0f, 1.0f, -1.0f,
                0, 0, 1,
                0 , 0,
                -1.0f, -1.0f, -1.0f,
                0, 0, 1,
                1 , 1,
                1.0f, 1.0f, -1.0f,
                0, 0, 1,
                0 , 0,
                -1.0f, 1.0f, -1.0f,
                0, 0, 1,
                1 , 0,
                //RIGHT
                1.0f, -1.0f, 1.0f,
                0, 0, 1,
                0 , 1,
                1.0f, -1.0f, -1.0f,
                0, 0, 1,
                1 , 1,
                1.0f, 1.0f, -1.0f,
                0, 0, 1,
                1 , 0,
                1.0f, -1.0f, 1.0f,
                0, 0, 1,
                0 , 1,
                1.0f, 1.0f, -1.0f,
                0, 0, 1,
                1 , 0,
                1.0f, 1.0f, 1.0f,
                0, 0, 1,
                0 , 0,
                //LEFT
                -1.0f, -1.0f, 1.0f,
                0, 0, 1,
                1 , 1,
                -1.0f, -1.0f, -1.0f,
                0, 0, 1,
                0 , 1,
                -1.0f, 1.0f, 1.0f,
                0, 0, 1,
                1 , 0,
                -1.0f, -1.0f, -1.0f,
                0, 0, 1,
                0 , 1,
                -1.0f, 1.0f, -1.0f,
                0, 0, 1,
                0 , 0,
                -1.0f, 1.0f, 1.0f,
                0, 0, 1,
                1 , 0,
                //UP
                -1.0f, 1.0f, 1.0f,
                0, 0, 1,
                1 , 1,
                1.0f, 1.0f, 1.0f,
                0, 0, 1,
                1 , 0,
                -1.0f, 1.0f, -1.0f,
                0, 0, 1,
                0 , 1,
                1.0f, 1.0f, 1.0f,
                0, 0, 1,
                1 , 0,
                1.0f, 1.0f, -1.0f,
                0, 0, 1,
                0 , 0,
                -1.0f, 1.0f, -1.0f,
                0, 0, 1,
                0 , 1,-
                //DOWN
                -1.0f, -1.0f, 1.0f,
                0, 0, 1,
                1, 0,
                1.0f, -1.0f, 1.0f,
                0, 0, 1,
                1, 1,
                -1.0f, -1.0f, -1.0f,
                0, 0, 1,
                0, 0,
                1.0f, -1.0f, 1.0f,
                0, 0, 1,
                1, 1,
                1.0f, -1.0f, -1.0f,
                0, 0, 1,
                0, 1,
                -1.0f, -1.0f, -1.0f,
                0, 0, 1,
                0, 0,
            };
            vertexBufferID = GPU.GenerateBuffer(verts);

            scaleMat = glm.scale(new mat4(1), new vec3(50.0f, 50.0f, 50.0f));

            //load tree
            tree = new Model3D();
            tree.LoadFile(projectPath + "\\ModelFiles\\static\\tree", "Tree.obj", 1);
            for (int i = 0; i < 6; i++)
            {
                xAxis = rand.Next(0, 700);
                xArr.Add(xAxis);
            }
            for (int i = 0; i < 6; i++)
            {
                zAxis = rand.Next(0, 550);
                zArr.Add(zAxis);
            }

            //camera
            cam = new Camera();
            //cam.Reset(0, 25, 45, 0, 0, 0, 0, 1, 0);//0, 34, 55, 0, 0, 0, 0, 1, 0
            ProjectionMatrix = cam.GetProjectionMatrix();
            ViewMatrix = cam.GetViewMatrix();

            transID = Gl.glGetUniformLocation(sh.ID, "model");
            projID = Gl.glGetUniformLocation(sh.ID, "projection");
            viewID = Gl.glGetUniformLocation(sh.ID, "view");

            

            //light
            DataID = Gl.glGetUniformLocation(sh.ID, "data");
            vec2 data = new vec2(100, 50);
            Gl.glUniform2fv(DataID, 1, data.to_array());
            int LightPositionID = Gl.glGetUniformLocation(sh.ID, "LightPosition_worldspace");
            vec3 lightPosition = new vec3(1.0f, 20f, 4.0f);
            Gl.glUniform3fv(LightPositionID, 1, lightPosition.to_array());
            //setup the ambient light component.
            AmbientLightID = Gl.glGetUniformLocation(sh.ID, "ambientLight");
            ambientLight = new vec3(0.2f, 0.18f, 0.01f);//0.2f, 0.18f, 0.01f
            Gl.glUniform3fv(AmbientLightID, 1, ambientLight.to_array());
            //setup the eye position.
            EyePositionID = Gl.glGetUniformLocation(sh.ID, "EyePosition_worldspace");

            //day
            //vec3 Day=
            Day = new vec3(1, 1, 0.7f);
            Night = new vec3(0.4f, 0.4f, 0.9f);
            
            
            
            shG.UseShader();
            transIDG = Gl.glGetUniformLocation(shG.ID, "model");
            projIDG = Gl.glGetUniformLocation(shG.ID, "projection");
            viewIDG = Gl.glGetUniformLocation(shG.ID, "view");
            groundIce = Gl.glGetUniformLocation(shG.ID, "ice");
            groundSand = Gl.glGetUniformLocation(shG.ID, "sand");
            groundGrass = Gl.glGetUniformLocation(shG.ID, "grass");
            groundRock = Gl.glGetUniformLocation(shG.ID, "rock");
            controlDrawn = Gl.glGetUniformLocation(shG.ID, "control");
            //water
            timeID = Gl.glGetUniformLocation(shG.ID, "time");
            groundWater = Gl.glGetUniformLocation(shG.ID, "water");
            waterG = new Ground(513, 513, 16, 8);
            //waterG = new Ground(projectPath);

        }
        public void Draw()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glDepthFunc(Gl.GL_LESS);

            shG.UseShader();
            Gl.glUniformMatrix4fv(transIDG, 1, Gl.GL_FALSE, scaleMat.to_array());
            Gl.glUniformMatrix4fv(projIDG, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());
            Gl.glUniformMatrix4fv(viewIDG, 1, Gl.GL_FALSE, ViewMatrix.to_array());

            //ice
            Gl.glActiveTexture(Gl.GL_TEXTURE0);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, ice.mtexture);
            Gl.glUniform1i(groundIce, 0);
            //rock
            Gl.glActiveTexture(Gl.GL_TEXTURE1);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, rock.mtexture);
            Gl.glUniform1i(groundRock, 1);
            //sand
            Gl.glActiveTexture(Gl.GL_TEXTURE2);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, sand.mtexture);
            Gl.glUniform1i(groundSand, 2);
            //grass
            Gl.glActiveTexture(Gl.GL_TEXTURE3);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, grass.mtexture);
            Gl.glUniform1i(groundGrass, 3);


            //water motion
            if (reverse)
                t -= 0.0002f;
            else
                t += 0.0002f;
            if (t > 0.2f)
                reverse = true;
            if (t < 0)
                reverse = false;
            Gl.glUniform1f(timeID, t);
            int testWater = Gl.glGetUniformLocation(shG.ID, "isWater");
            float wa = 1;
            
            Gl.glUniform1f(controlDrawn, 2);
            Gl.glUniform1f(testWater, wa);
            //water texture and blending
            Gl.glActiveTexture(Gl.GL_TEXTURE4);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, water.mtexture);
            Gl.glUniform1i(groundWater, 4);
            
            waterG.Draw(transIDG);

            wa = 0;
            Gl.glUniform1f(testWater, wa);
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);

            Gl.glUniform1f(controlDrawn, 1);

            grond.Draw(transIDG);

            //draw tree model
            for (int i = 0; i < 6; i++)
            {
                tree.transmatrix = glm.translate(new mat4(1), new vec3((xArr[i] * 0.195f) - 50, ((float)grond.heights[(int)xArr[i], (int)zArr[i]] * 0.3f) - 50, (zArr[i] * 0.195f) - 50));
                tree.scalematrix = glm.scale(new mat4(1), new vec3(3.8f, 3.8f, 3.8f));
                tree.Draw(transIDG);
            }

            sh.UseShader();


            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, scaleMat.to_array());
            Gl.glUniformMatrix4fv(projID, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());
            Gl.glUniformMatrix4fv(viewID, 1, Gl.GL_FALSE, ViewMatrix.to_array());

            GPU.BindBuffer(vertexBufferID);
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), IntPtr.Zero);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(3 * sizeof(float)));
            Gl.glEnableVertexAttribArray(2);
            Gl.glVertexAttribPointer(2, 2, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(6 * sizeof(float)));

            front.Bind();
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);

            back.Bind();
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 6, 6);

            left.Bind();
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 12, 6);

            right.Bind();
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 18, 6);

            up.Bind();
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 24, 6);

            down.Bind();
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 30, 6);
            
            
            ambientLight = Day + (Night - Day) * (time % 1);
            time += 0.0001f;
            Gl.glUniform3fv(AmbientLightID, 1, ambientLight.to_array());
                       
            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);
            Gl.glDisableVertexAttribArray(2);
        }
        public void Update()
        {
            cam.UpdateViewMatrix();
            ProjectionMatrix = cam.GetProjectionMatrix();
            ViewMatrix = cam.GetViewMatrix();
        }
        public void CleanUp()
        {
            sh.DestroyShader();
        }
    }

}