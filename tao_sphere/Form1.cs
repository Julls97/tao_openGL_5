using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Tao.OpenGl;
using Tao.FreeGlut;

namespace tao_sphere {
	public partial class Form1 : Form {
		public static float x = 0, y = 0;
		float[] color = new float[4] { 0, 1, 0, 1 }; // красный цвет 
		float[] shininess = new float[1] { 10 };

		public Form1() {
			InitializeComponent();
			simpleOpenGlControl1.InitializeContexts();

			Glut.glutInit();
			Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
			Gl.glViewport(0, 0, simpleOpenGlControl1.Width, simpleOpenGlControl1.Height);
			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glLoadIdentity();
			Glu.gluPerspective(45.0f, (float)simpleOpenGlControl1.Width / (float)simpleOpenGlControl1.Height, 0.1f, 200.0f);
			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			Gl.glLoadIdentity();
			Gl.glEnable(Gl.GL_DEPTH_TEST);

			Gl.glEnable(Gl.GL_LIGHTING);
			Gl.glLightModelf(Gl.GL_LIGHT_MODEL_TWO_SIDE, Gl.GL_TRUE); // делаем так, чтобы освещались обе стороны полигона 
			Gl.glEnable(Gl.GL_NORMALIZE); //делам нормали одинаковой величины во избежание артефактов
			
			DrawScene();
		}

		public int light_sample = 0;

		private void DrawScene() {
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
			Gl.glLoadIdentity();

			Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_DIFFUSE, color); // цвет  
			Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SPECULAR, color); // отраженный свет 
			Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SHININESS, shininess); // степень отраженного света

			float[] material_diffuse = new float[4]{ 1, 0, 0, 1 };
			Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_DIFFUSE, material_diffuse);

			if (light_sample == 0) {
				float[] light1_diffuse = new float[3] { 0.4f, 0.7f, 0.2f };
				float[] light1_position = new float[4] { x, y, 1.0f, 0.0f }; // устанавливаем направление света

				Gl.glEnable(Gl.GL_LIGHT2);
				Gl.glLightfv(Gl.GL_LIGHT2, Gl.GL_DIFFUSE, light1_diffuse);
				Gl.glLightfv(Gl.GL_LIGHT2, Gl.GL_POSITION, light1_position);
			}

			if (light_sample == 1) {
				float[] light0_diffuse = new float[3] { 0.4f, 0.7f, 0.2f };
				float[] light0_position = new float[4] { x, y, 1.0f, 1.0f }; // устанавливаем направление света

				Gl.glEnable(Gl.GL_LIGHT0);
				Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_DIFFUSE, light0_diffuse);
				Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, light0_position);
			}

			if (light_sample == 2) {
				float[] light1_diffuse = new float[3] { 0.4f, 0.7f, 0.2f };
				float[] light1_position = new float[4] { x, y, 1.0f, 1.0f }; // устанавливаем направление света
				float[] light1_spot_direction = new float[3] { 0, 0, -1 };

				Gl.glEnable(Gl.GL_LIGHT1);
				Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_DIFFUSE, light1_diffuse);
				Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_POSITION, light1_position);
				Gl.glLightf(Gl.GL_LIGHT1, Gl.GL_SPOT_CUTOFF, 3);
				Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_SPOT_DIRECTION, light1_spot_direction);
			}

			Gl.glPushMatrix();
			Gl.glColor3f(1, 0, 0);
			Gl.glTranslated(0, 0, -6);
			Gl.glRotated(90, 1, 0, 0);
			Glut.glutSolidSphere(2, 32, 32);
			Gl.glPopMatrix();



			Gl.glDisable(Gl.GL_LIGHT0);
			Gl.glDisable(Gl.GL_LIGHT1);
			Gl.glDisable(Gl.GL_LIGHT2);



			Gl.glFlush();
			simpleOpenGlControl1.Invalidate();
		}

		//float[] light0_diffuse = new float[3] { 1, 1, 1 }; // устанавливаем диффузный цвет света 	
		//float[] light0_position = new float[4] { x, y, 1.0f, 0.0f }; // устанавливаем направление света

		private static void SetLight(float[] light_diffuse, float[] light_position) {
		//	Gl.glEnable(Gl.GL_LIGHTING); 
			Gl.glEnable(Gl.GL_LIGHT0);

			//float[] light0_diffuse = new float[3] { 1, 1, 1 }; // устанавливаем диффузный цвет света 						
			//float[] light0_position = new float[4] { x, y, 1.0f, 0.0f }; // устанавливаем направление света 
			Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_DIFFUSE, light_diffuse); // устанавливаем источнику света light0 диффузный свет, который указали ранее 
			Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, light_position); // устанавливаем направление источника света, указанное ранее 
			
			// Если значение w = 0, то источник света бесконечно удаленный (что-то вроде солнца). 
			// Если w = 1, то этот источник света точечный (что-то вроде лампочки). 
			
		}

		private void SetMaterial() {
			Gl.glEnable(Gl.GL_LIGHTING);
			Gl.glEnable(Gl.GL_LIGHT0);


			//Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_DIFFUSE, color); // цвет  
			//Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SPECULAR, color); // отраженный свет 
			//Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SHININESS, shininess); // степень отраженного света
		}

		private void simpleOpenGlControl1_KeyPress(object sender, KeyPressEventArgs e) {
			float step = 0.4f;

			if (e.KeyChar == 'A' || e.KeyChar == 'a') x -= step;
			if (e.KeyChar == 'D' || e.KeyChar == 'd') x += step;
			if (e.KeyChar == 'S' || e.KeyChar == 's') y -= step;
			if (e.KeyChar == 'W' || e.KeyChar == 'w') y += step;

		//	SetLight();
			DrawScene();
		}

		private void simpleOpenGlControl1_KeyUp(object sender, KeyEventArgs e) {

			if (e.KeyCode == Keys.Escape) Close();

			if (e.KeyCode == Keys.Q) { // включает материал
									   	SetMaterial();
									 	//SetLight(light0_diffuse, light0_position);
				
				DrawScene();
			}

			//if (e.KeyCode == Keys.L) { // включает точечный свет
			//	float[] light1_diffuse = new float[3] { 0.7f, 0.7f, 0.7f };
			//	float[] light1_position = new float[4] { x, y, 1.0f, 1.0f };

			//	//Gl.glDisable(Gl.GL_LIGHT0);
			//	//Gl.glEnable(Gl.GL_LIGHT1);
			//	//Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_DIFFUSE, light1_diffuse);
			//	//Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_POSITION, light1_position);

			//	SetLight(light1_diffuse, light1_position);

			//	DrawScene();
			//}

			//if (e.KeyCode == Keys.P) { // включает прожектор
			//	float[] light2_diffuse = new float[3]{ 0.4f, 0.7f, 0.2f };
			//	float[] light2_position = new float[4]{ x, y, 1.0f, 1.0f };
			//	float[] light2_spot_direction = new float[3]{ 0.0f, 0.0f, -1.0f };

			//	Gl.glDisable(Gl.GL_LIGHT0);
			//	Gl.glEnable(Gl.GL_LIGHT2);
			//	Gl.glLightfv(Gl.GL_LIGHT2, Gl.GL_DIFFUSE, light2_diffuse);
			//	Gl.glLightfv(Gl.GL_LIGHT2, Gl.GL_POSITION, light2_position);
			//	Gl.glLightf(Gl.GL_LIGHT2, Gl.GL_SPOT_CUTOFF, 30);
			//	Gl.glLightfv(Gl.GL_LIGHT2, Gl.GL_SPOT_DIRECTION, light2_spot_direction);

			//	DrawScene();
			//}

			//if (e.KeyCode == Keys.F) {  // включает фоновый свет

			//}

			if (e.KeyCode == Keys.R) { light_sample = 0; DrawScene(); }
			if (e.KeyCode == Keys.T) { light_sample = 1; DrawScene(); }
			if (e.KeyCode == Keys.Y) {light_sample = 2; DrawScene(); }


		}
	}
}
