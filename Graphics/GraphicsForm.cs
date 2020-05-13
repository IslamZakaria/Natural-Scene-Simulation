using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Diagnostics;
using System;
using GlmNet;

namespace Graphics
{
    public partial class GraphicsForm : Form
    {
        Renderer renderer = new Renderer();
        Thread MainLoopThread;

        float deltaTime;
        public GraphicsForm()
        {
            InitializeComponent();
            simpleOpenGlControl1.InitializeContexts();

            //MoveCursor();
            

            initialize();
            deltaTime = 0.005f;
            MainLoopThread = new Thread(MainLoop);
            MainLoopThread.Start();

        }
        void initialize()
        {
            renderer.Initialize();   
        }
        void MainLoop()
        {
            while (true)
            {
                renderer.Update();
                renderer.Draw();
                simpleOpenGlControl1.Refresh();
            }
        }
        private void GraphicsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            renderer.CleanUp();
            MainLoopThread.Abort();
        }

        private void simpleOpenGlControl1_Paint(object sender, PaintEventArgs e)
        {
            renderer.Draw();
        }

        private void simpleOpenGlControl1_KeyPress(object sender, KeyPressEventArgs e)
        {
            float x;
            float y;
            float z;
            vec3 pos = renderer.cam.mPosition;
            float speed = 1.6f;
            if (e.KeyChar == 'a')
                renderer.cam.Strafe(-speed);
            if (e.KeyChar == 'd')
                renderer.cam.Strafe(speed);
            if (e.KeyChar == 's')
                renderer.cam.Walk(-speed);
            if (e.KeyChar == 'w')
                renderer.cam.Walk(speed);
            /*if (e.KeyChar == 'z')
                renderer.cam.Fly(-speed);
            if (e.KeyChar == 'c')
                renderer.cam.Fly(speed);*/
            
            
            if ((renderer.cam.mPosition.x >= renderer.grond.xMin && renderer.cam.mPosition.x <= renderer.grond.xMax) && (renderer.cam.mPosition.y >= renderer.grond.yMin && renderer.cam.mPosition.y <= renderer.grond.yMax) && (renderer.cam.mPosition.z >= renderer.grond.zMin && renderer.cam.mPosition.z <= renderer.grond.zMax))
            {
                x = renderer.cam.mPosition.x;
                x = (x + 50) / (float)0.195;
                z = renderer.cam.mPosition.z;
                z = (z + 50) / (float)0.195;
                y = renderer.grond.heights[(int)x, (int)z];
                y = (y * (float)0.3) - 50;
                renderer.cam.mPosition.y = y + 5;    
            }
            else
            {
                renderer.cam.mPosition = pos;
            }

            label6.Text = "X: " + renderer.cam.GetCameraPosition().x;
            label7.Text = "Y: " + renderer.cam.GetCameraPosition().y;
            label8.Text = "Z: " + renderer.cam.GetCameraPosition().z;
            label6.Show();
            label7.Show();
            label8.Show();
        }



        float prevX, prevY;
        private void simpleOpenGlControl1_MouseMove(object sender, MouseEventArgs e)
        {
            float speed = 0.05f;

            float delta = e.X - prevX;
            if (delta > 2)
                renderer.cam.Yaw(-speed);
            else if (delta < -2)
                renderer.cam.Yaw(speed);


            delta = e.Y - prevY;
            if (delta > 2)
                renderer.cam.Pitch(-speed);
            else if (delta < -2)
                renderer.cam.Pitch(speed);

            MoveCursor();
        }

        
        private void MoveCursor()
        {
            this.Cursor = new Cursor(Cursor.Current.Handle);
            Point p = PointToScreen(simpleOpenGlControl1.Location);
            Cursor.Position = new Point(simpleOpenGlControl1.Size.Width/2+p.X, simpleOpenGlControl1.Size.Height/2+p.Y);
            Cursor.Clip = new Rectangle(this.Location, this.Size);
            prevX = simpleOpenGlControl1.Location.X+simpleOpenGlControl1.Size.Width/2;
            prevY = simpleOpenGlControl1.Location.Y + simpleOpenGlControl1.Size.Height / 2;
        }

        private void GraphicsForm_Load(object sender, EventArgs e)
        {

        }

        private void simpleOpenGlControl1_Load(object sender, EventArgs e)
        {
            
        }

        private void tour_Click(object sender, EventArgs e)
        {
            float x;
            float y;
            float z;
            vec3 pos = renderer.cam.mPosition;
            float speed = 1.6f;
            int walk = 0;
            int straf = 0;
            //walk 30
            int i = 30;
            while (i>0)
            {
                i--;
                if ((renderer.cam.mPosition.x >= renderer.grond.xMin && renderer.cam.mPosition.x <= renderer.grond.xMax) && (renderer.cam.mPosition.y >= renderer.grond.yMin && renderer.cam.mPosition.y <= renderer.grond.yMax) && (renderer.cam.mPosition.z >= renderer.grond.zMin && renderer.cam.mPosition.z <= renderer.grond.zMax))
                {
                    x = renderer.cam.mPosition.x;
                    x = (x + 50) / (float)0.195;
                    z = renderer.cam.mPosition.z;
                    z = (z + 50) / (float)0.195;
                    y = renderer.grond.heights[(int)x, (int)z];
                    y = (y * (float)0.3) - 50;
                    renderer.cam.mPosition.y = y + 5;
                    renderer.cam.Walk(speed);
                    walk++;
                }
                else
                {
                    renderer.cam.mPosition = pos;
                    break;
                }
            }
            //strafe 30
            i = 30;
            while (i > 0)
            {
                i--;
                if ((renderer.cam.mPosition.x >= renderer.grond.xMin && renderer.cam.mPosition.x <= renderer.grond.xMax) && (renderer.cam.mPosition.y >= renderer.grond.yMin && renderer.cam.mPosition.y <= renderer.grond.yMax) && (renderer.cam.mPosition.z >= renderer.grond.zMin && renderer.cam.mPosition.z <= renderer.grond.zMax))
                {
                    x = renderer.cam.mPosition.x;
                    x = (x + 50) / (float)0.195;
                    z = renderer.cam.mPosition.z;
                    z = (z + 50) / (float)0.195;
                    y = renderer.grond.heights[(int)x, (int)z];
                    y = (y * (float)0.3) - 50;
                    renderer.cam.mPosition.y = y + 5;
                    renderer.cam.Strafe(speed);
                    straf++;
                }
                else
                {
                    renderer.cam.mPosition = pos;
                    break;
                }
            }
            //walk back 30
            
            while (walk > 0)
            {
                walk--;
                if ((renderer.cam.mPosition.x >= renderer.grond.xMin && renderer.cam.mPosition.x <= renderer.grond.xMax) && (renderer.cam.mPosition.y >= renderer.grond.yMin && renderer.cam.mPosition.y <= renderer.grond.yMax) && (renderer.cam.mPosition.z >= renderer.grond.zMin && renderer.cam.mPosition.z <= renderer.grond.zMax))
                {
                    x = renderer.cam.mPosition.x;
                    x = (x + 50) / (float)0.195;
                    z = renderer.cam.mPosition.z;
                    z = (z + 50) / (float)0.195;
                    y = renderer.grond.heights[(int)x, (int)z];
                    y = (y * (float)0.3) - 50;
                    renderer.cam.mPosition.y = y + 5;
                    renderer.cam.Walk(-speed);
                }
                else
                {
                    renderer.cam.mPosition = pos;
                    break;
                }
            }
            
            //straf back 30
            while (straf > 0)
            {
                straf--;
                if ((renderer.cam.mPosition.x >= renderer.grond.xMin && renderer.cam.mPosition.x <= renderer.grond.xMax) && (renderer.cam.mPosition.y >= renderer.grond.yMin && renderer.cam.mPosition.y <= renderer.grond.yMax) && (renderer.cam.mPosition.z >= renderer.grond.zMin && renderer.cam.mPosition.z <= renderer.grond.zMax))
                {
                    x = renderer.cam.mPosition.x;
                    x = (x + 50) / (float)0.195;
                    z = renderer.cam.mPosition.z;
                    z = (z + 50) / (float)0.195;
                    y = renderer.grond.heights[(int)x, (int)z];
                    y = (y * (float)0.3) - 50;
                    renderer.cam.mPosition.y = y + 5;
                    renderer.cam.Strafe(-speed);
                }
                else
                {
                    renderer.cam.mPosition = pos;
                    break;
                }
            }
        }
    }
}
