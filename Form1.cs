using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Windows.Forms;
using OpenCvSharp;

namespace _028_01_칼라영상처리2_DB연동_
{
    public partial class Form1 : Form
    {
        //////////////////////////////////
        //      전역 변수
        //////////////////////////////////
        static byte[,,] inImage = null, outImage = null, tmpImage = null;
        static int inH = 0, inW = 0, outH = 0, outW = 0, tmpH = 0, tmpW = 0;
        static string fileName = null, openFileName = null;
        static Bitmap paper, bitmap, image;
        const int RGB = 3, RR = 0, GG = 1, BB = 2;
        const double MAXSIZE = 750;
        static int mouseYN = 0;
        static int down_x = 0, down_y = 0, up_x = 0, up_y = 0;
        static string[] tmpFiles = new string[100];
        static int tmpIndex = 0;
        static Mat inCvImage, outCvImage, tmpCvImage;

        //////////////////////////////////
        //      이벤트 함수
        //////////////////////////////////
        public Form1()
        {
            InitializeComponent();
            toolStripStatusLabel1.Text = " ";
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // 실수로 누름
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 실수로 누름
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.O:
                        open_Image();
                        break;
                    case Keys.S:
                        save_Image();
                        break;
                    case Keys.Z:
                        restoreTempFile();
                        break;
                }
            }
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (mouseYN != 0)
            {
                down_x = e.Y;
                down_y = e.X;
            }
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseYN != 0)
            {
                up_x = e.Y;
                up_y = e.X;

                if (up_x < down_x)
                {
                    int tmp = up_x;
                    up_x = down_x;
                    down_x = tmp;
                }
                if (up_y < down_y)
                {
                    int tmp = up_y;
                    up_y = down_y;
                    down_y = tmp;
                }
                switch (mouseYN)
                {
                    case 1:
                        bright_Image();
                        break;
                    case 2:
                        color_Reverse_Image();
                        break;
                    case 3:
                        bw_Image();
                        break;
                }

                mouseYN = 0;
            }
        }
        private void 열기CtrlOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            open_Image();
        }
        private void 저장CtrlSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save_Image();
        }
        private void dB연동ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            db_Image();
        }
        private void 되돌리기CtrlZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            restoreTempFile();
        }
        private void 도움말ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("영상처리 프로그램 Ver2.2");
        }
        private void 원본ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            original_Image();
        }
        private void 전체지정ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bright_Image();
        }
        private void 마우스로지정ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            mouseYN = 1;
        }
        private void 배확대ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            scaleUp_Image();
        }
        private void 배축소ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            scaleDown_Image();
        }
        private void 전체지정ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            bw_Image();
        }
        private void 마우스로지정ToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            mouseYN = 3;
        }
        private void 좌우반전ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            rl_Reverse_Image();
        }
        private void 상하반전ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ud_Reverse_Image();
        }
        private void 전체지정ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            color_Reverse_Image();
        }
        private void 마우스로지정ToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            mouseYN = 2;
        }
        private void 이동ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            move_Image();
        }
        private void 회전ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            rotate_Image();
        }
        private void 경계선검출ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            edge_Image();
        }
        private void 약하게ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            mosaic1_Image();
        }
        private void 강하게ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            mosaic2_Image();
        }
        private void 엠보싱ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            emboss_Image();
        }
        private void 블러링ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            blurr_Image();
        }
        private void 스트레칭ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            stretch_Image();
        }
        private void 엔드인탐색ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            endin_Image();
        }
        private void 평활화ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            hisequal_Image();
        }
        private void 적응형이진화ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bw_CvImage();
        }
        private void 자르기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cut_CvImage();
        }
        private void 경계선추출ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            edge_CvImage();
        }
        private void 모폴로지ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            morp_CvImage();
        }
        private void 코너검출ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            corner_CvImage();
        }
        private void 원검출ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            circle_CvImage();
        }
        private void 색상검출ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            color_CvImage();
        }
        private void 중심점추출ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            center_CvImage();
        }
        private void 경계사각형추출ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            quadrangle_CvImage();
        }

        //////////////////////////////////
        //      공통 함수
        //////////////////////////////////
        void open_Image()
        {       // 이미지 불러오기
            label1.Visible = false;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "칼라 필터|*.png; *.jpg; *.bmp; *.tif";
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                if (outImage == null)
                    label1.Visible = true;
                return;
            }
            fileName = ofd.FileName;
            openFileName = ofd.FileName;

            inCvImage = Cv2.ImRead(fileName);
            tmpCvImage = Cv2.ImRead(fileName);
            Cv2.Transpose(inCvImage, inCvImage);
            Cv2.Transpose(tmpCvImage, tmpCvImage);

            inH = tmpH = inCvImage.Height;
            inW = tmpW = inCvImage.Width;
            inImage = new byte[RGB, inH, inW];
            tmpImage = new byte[RGB, tmpH, tmpW];
            for (int i = 0; i < inH; i++)
            {
                for (int k = 0; k < inW; k++)
                {
                    var c = inCvImage.At<Vec3b>(i, k);
                    inImage[RR, i, k] = tmpImage[RR, i, k] = c.Item2;
                    inImage[GG, i, k] = tmpImage[GG, i, k] = c.Item1;
                    inImage[BB, i, k] = tmpImage[BB, i, k] = c.Item0;
                }
            }
            equal_Image();
        }
        void save_Image()
        {       // 이미지 저장
            if (outImage == null)
            {
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = "";
            sfd.Filter = "PNG File(*.png) | *.png";
            if (sfd.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            string saveFileName = sfd.FileName;
            Bitmap image = new Bitmap(outH, outW);

            for (int i = 0; i < outH; i++)
            {
                for (int k = 0; k < outW; k++)
                {
                    Color c;
                    int r, g, b;
                    r = outImage[RR, i, k];
                    g = outImage[GG, i, k];
                    b = outImage[BB, i, k];
                    c = Color.FromArgb(r, g, b);
                    image.SetPixel(i, k, c);
                }
            }
            image.Save(saveFileName, ImageFormat.Png);
            toolStripStatusLabel1.Text = saveFileName + "으로 저장됨";
        }
        void saveTempFile()
        {       // tmpImage를 디스크에 저장 후, outImage를 tmpImage에 저장
            string saveFileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".tmp";
            image = new Bitmap(tmpH, tmpW);

            for (int i = 0; i < tmpH; i++)
            {
                for (int k = 0; k < tmpW; k++)
                {
                    int r = tmpImage[RR, i, k];
                    int g = tmpImage[GG, i, k];
                    int b = tmpImage[BB, i, k];
                    Color c = Color.FromArgb(r, g, b);
                    image.SetPixel(i, k, c);
                }
            }
            image.Save(saveFileName, System.Drawing.Imaging.ImageFormat.Png);
            tmpFiles[tmpIndex++] = saveFileName;

            tmpH = outH;
            tmpW = outW;
            tmpImage = new byte[RGB, tmpH, tmpW];
            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < tmpH; i++)
                {
                    for (int k = 0; k < tmpW; k++)
                    {
                        tmpImage[rgb, i, k] = outImage[rgb, i, k];
                    }
                }
            }
        }
        void restoreTempFile()
        {       // 디스크의 최근 파일을 tmpImage에 복사
            if (tmpIndex <= 0)
            {
                return;
            }
            fileName = tmpFiles[--tmpIndex];
            bitmap = new Bitmap(fileName);
            tmpW = outW = bitmap.Height;
            tmpH = outH = bitmap.Width;
            tmpImage = new byte[RGB, tmpH, tmpW];
            outImage = new byte[RGB, tmpH, tmpW];

            for (int i = 0; i < tmpH; i++)
            {
                for (int k = 0; k < tmpW; k++)
                {
                    Color c = bitmap.GetPixel(i, k);
                    outImage[RR, i, k] = tmpImage[RR, i, k] = c.R;
                    outImage[GG, i, k] = tmpImage[GG, i, k] = c.G;
                    outImage[BB, i, k] = tmpImage[BB, i, k] = c.B;
                }
            }
            bitmap.Dispose();
            System.IO.File.Delete(fileName); // 임시파일 삭제

            display_Image();
            OutImageToCv();
        }
        void display_Image()
        {       // 이미지 출력
            int oW = outW, oH = outH;
            double hop = 1.0;

            if (oH > MAXSIZE || oW > MAXSIZE)
            {
                if (outW > outH)
                {
                    hop = (outW / MAXSIZE);
                }
                else
                {
                    hop = (outH / MAXSIZE);
                }

                oW = (int)(outW / hop);
                oH = (int)(outH / hop);
            }

            paper = new Bitmap(oH, oW);
            pictureBox1.Size = new System.Drawing.Size(oH, oW);
            this.Size = new System.Drawing.Size(oH + 150, oW + 110);

            Color pen;
            for (int i = 0; i < outH; i++)
            {
                for (int k = 0; k < outW; k++)
                {
                    if (i >= oH - 1 || k >= oW - 1)
                    {
                        continue;
                    }

                    byte r = outImage[RR, (int)(i * hop), (int)(k * hop)];
                    byte g = outImage[GG, (int)(i * hop), (int)(k * hop)];
                    byte b = outImage[BB, (int)(i * hop), (int)(k * hop)];
                    pen = Color.FromArgb(r, g, b);
                    paper.SetPixel(i, k, pen);
                }
            }
            pictureBox1.Image = paper;
            toolStripStatusLabel1.Text = "(" + outH.ToString() + "x" + outW.ToString() + ")  " + fileName;
        }
        double getValue(string label_Text, int max, int min)
        {       // 서브폼의 입력값 반환
            subForm01 sf01 = new subForm01(label_Text, max, min);
            if (sf01.ShowDialog() == DialogResult.Cancel)
            {
                return 0;
            }
            double value = (double)sf01.nud.Value;
            return value;
        }
        void db_Image()
        {       // outImage와 DB를 연동
            dbForm df = new dbForm(openFileName);
            df.outH = outH;
            df.outW = outW;
            df.outImage = outImage;

            if (df.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            fileName = df.db_Server + "\\blob_db\\blob_tb\\" + df.f_name + "." + df.f_extname;
            label1.Visible = false;

            inH = tmpH = outH = df.outH;
            inW = tmpW = outW = df.outW;
            inImage = new byte[RGB, inH, inW];
            tmpImage = new byte[RGB, tmpH, tmpW];
            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < outH; i++)
                {
                    for (int k = 0; k < outW; k++)
                    {
                        inImage[rgb, i, k] = df.outImage[rgb, i, k];
                        tmpImage[rgb, i, k] = df.outImage[rgb, i, k];
                    }
                }
            }
            equal_Image();
        }
        void CvToOutImage()
        {
            outH = outCvImage.Height;
            outW = outCvImage.Width;
            outImage = new byte[RGB, outH, outW];

            for (int i = 0; i < outH; i++)
                for (int k = 0; k < outW; k++)
                {
                    var c = outCvImage.At<Vec3b>(i, k);
                    outImage[RR, i, k] = c.Item2;
                    outImage[GG, i, k] = c.Item1;
                    outImage[BB, i, k] = c.Item0;
                }
        }
        void OutImageToCv()
        {
            string tmpFileName = System.IO.Path.GetTempPath() + "CV.tmp";
            Bitmap image = new Bitmap(outH, outW);

            for (int i = 0; i < outH; i++)
            {
                for (int k = 0; k < outW; k++)
                {
                    Color c;
                    int r, g, b;
                    r = outImage[RR, i, k];
                    g = outImage[GG, i, k];
                    b = outImage[BB, i, k];
                    c = Color.FromArgb(r, g, b);
                    image.SetPixel(i, k, c);
                }
            }
            image.Save(tmpFileName, ImageFormat.Png);

            tmpCvImage = Cv2.ImRead(tmpFileName);
            Cv2.Transpose(tmpCvImage, tmpCvImage);
            outCvImage = Cv2.ImRead(tmpFileName);
            Cv2.Transpose(outCvImage, outCvImage);

            image.Dispose();
            System.IO.File.Delete(tmpFileName);
        }

        //////////////////////////////////
        //      영상처리 함수
        //////////////////////////////////
        void equal_Image()
        {       // tmpImage 출력
            if (inImage == null)
            {
                return;
            }

            outH = tmpH;
            outW = tmpW;
            outImage = new byte[RGB, outH, outW];

            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < outH; i++)
                {
                    for (int k = 0; k < outW; k++)
                    {
                        outImage[rgb, i, k] = tmpImage[rgb, i, k];
                    }
                }
            }
            display_Image();
            saveTempFile();
            OutImageToCv();
        }
        void original_Image()
        {       // 원본 출력
            if (inImage == null)
            {
                return;
            }

            outH = tmpH = inH;
            outW = tmpW = inW;
            outImage = new byte[RGB, outH, outW];

            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < outH; i++)
                {
                    for (int k = 0; k < outW; k++)
                    {
                        tmpImage[rgb, i, k] = inImage[rgb, i, k];
                        outImage[rgb, i, k] = inImage[rgb, i, k];
                    }
                }
            }
            display_Image();
            saveTempFile();
            OutImageToCv();
        }
        void bright_Image()
        {       // 밝기 조절
            if (inImage == null)
            {
                return;
            }

            outH = tmpH;
            outW = tmpW;
            outImage = new byte[RGB, outH, outW];

            if (up_y > outH || up_x > outW)
            {
                return;
            }
            if (mouseYN == 0)
            {
                down_x = 0;
                down_y = 0;
                up_x = outW;
                up_y = outH;
            }

            int bright = (int)getValue("밝기를 입력하세요. (+:밝게/-:어둡게)", 255, -255);

            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < outH; i++)
                {
                    for (int k = 0; k < outW; k++)
                    {
                        if ((down_x <= k && k <= up_x) && (down_y <= i && i <= up_y))
                        {
                            if ((tmpImage[rgb, i, k] + bright) > 255)
                            {
                                outImage[rgb, i, k] = 255;
                            }
                            else if ((tmpImage[rgb, i, k] + bright) < 0)
                            {
                                outImage[rgb, i, k] = 0;
                            }
                            else
                            {
                                outImage[rgb, i, k] = (byte)(tmpImage[rgb, i, k] + bright);
                            }
                        }
                        else
                        {
                            outImage[rgb, i, k] = tmpImage[rgb, i, k];
                        }
                    }
                }
            }
            display_Image();
            saveTempFile();
            OutImageToCv();
        }
        void bw_Image()
        {       // 흑백 출력 (픽셀의 평균값 기준)
            if (inImage == null)
            {
                return;
            }

            outH = tmpH;
            outW = tmpW;
            outImage = new byte[RGB, outH, outW];

            if (up_y > outH || up_x > outW)
            {
                return;
            }
            if (mouseYN == 0)
            {
                down_x = 0;
                down_y = 0;
                up_x = outW;
                up_y = outH;
            }

            int sum = 0;
            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < outH; i++)
                {
                    for (int k = 0; k < outW; k++)
                    {
                        sum += tmpImage[rgb, i, k];
                    }
                }
            }
            double avg = (double)sum / (outH * outW * RGB);
            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < outH; i++)
                {
                    for (int k = 0; k < outW; k++)
                    {
                        double rgb_avg = (tmpImage[RR, i, k] + tmpImage[GG, i, k] + tmpImage[BB, i, k]) / RGB;
                        if ((down_x <= k && k <= up_x) && (down_y <= i && i <= up_y))
                        {
                            if (rgb_avg < avg)
                            {
                                outImage[rgb, i, k] = 0;
                            }
                            else
                            {
                                outImage[rgb, i, k] = 255;
                            }
                        }
                        else
                        {
                            outImage[rgb, i, k] = tmpImage[rgb, i, k];
                        }
                    }
                }
            }
            display_Image();
            saveTempFile();
            OutImageToCv();
        }
        void color_Reverse_Image()
        {       // 색상 반전 출력
            if (inImage == null)
            {
                return;
            }

            outH = tmpH;
            outW = tmpW;
            outImage = new byte[RGB, outH, outW];

            if (up_y > outH || up_x > outW)
            {
                return;
            }
            if (mouseYN == 0)
            {
                down_x = 0;
                down_y = 0;
                up_x = outW;
                up_y = outH;
            }

            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < outH; i++)
                {
                    for (int k = 0; k < outW; k++)
                    {
                        if ((down_x <= k && k <= up_x) && (down_y <= i && i <= up_y))
                        {
                            outImage[rgb, i, k] = (byte)(255 - tmpImage[rgb, i, k]);
                        }
                        else
                        {
                            outImage[rgb, i, k] = tmpImage[rgb, i, k];
                        }
                    }
                }
            }
            display_Image();
            saveTempFile();
            OutImageToCv();
        }
        void rl_Reverse_Image()
        {       // 좌우 반전 출력
            if (inImage == null)
            {
                return;
            }

            outH = tmpH;
            outW = tmpW;
            outImage = new byte[RGB, outH, outW];

            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < outH; i++)
                {
                    for (int k = 0; k < outW; k++)
                    {
                        outImage[rgb, i, k] = tmpImage[rgb, outH - i - 1, k];
                    }
                }
            }
            display_Image();
            saveTempFile();
            OutImageToCv();
        }
        void ud_Reverse_Image()
        {       // 상하 반전 출력
            if (inImage == null)
            {
                return;
            }

            outH = tmpH;
            outW = tmpW;
            outImage = new byte[RGB, outH, outW];

            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < outH; i++)
                {
                    for (int k = 0; k < outW; k++)
                    {
                        outImage[rgb, i, k] = tmpImage[rgb, i, outW - k - 1];
                    }
                }
            }
            display_Image();
            saveTempFile();
            OutImageToCv();
        }
        void scaleUp_Image()
        {       // 크기 조절(확대)
            if (inImage == null)
            {
                return;
            }

            outH = tmpH * 2;
            outW = tmpW * 2;
            outImage = new byte[RGB, outH, outW];

            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < outH; i++)
                {
                    for (int k = 0; k < outW; k++)
                    {
                        outImage[rgb, i, k] = tmpImage[rgb, i / 2, k / 2];
                    }
                }
            }
            display_Image();
            saveTempFile();
            OutImageToCv();
        }
        void scaleDown_Image()
        {       // 크기 조절(축소)
            if (inImage == null)
            {
                return;
            }

            outH = tmpH / 2;
            outW = tmpW / 2;
            outImage = new byte[RGB, outH, outW];

            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < outH; i++)
                {
                    for (int k = 0; k < outW; k++)
                    {
                        outImage[rgb, i, k] = tmpImage[rgb, i * 2, k * 2];
                    }
                }
            }
            display_Image();
            saveTempFile();
            OutImageToCv();
        }
        void move_Image()
        {       // 이동 출력
            if (inImage == null)
            {
                return;
            }

            outH = tmpH;
            outW = tmpW;
            outImage = new byte[RGB, outH, outW];

            int y = (int)getValue("이동 방향과 거리를 입력하세요. (+:우/-:좌)", outW, -outW);
            int x = (int)getValue("이동 방향과 거리를 입력하세요. (+:하/-:상)", outH, -outH);

            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < outH; i++)
                {
                    for (int k = 0; k < outW; k++)
                    {
                        outImage[rgb, i, k] = 0;
                    }
                }
            }

            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < outH; i++)
                {
                    if ((i + y) < 0 || (i + y) >= outH)
                    {
                        continue;
                    }
                    for (int k = 0; k < outW; k++)
                    {
                        if ((k + x) < 0 || (k + x) >= outW)
                        {
                            continue;
                        }
                        outImage[rgb, i + y, k + x] = tmpImage[rgb, i, k];
                    }
                }
            }
            display_Image();
            saveTempFile();
            OutImageToCv();
        }
        void rotate_Image()
        {       // 회전 출력
            if (inImage == null)
            {
                return;
            }

            double deg = (double)getValue("회전할 각도를 입력하세요. (시계방향)", 360, 0);
            double PI = 3.141592;
            double rad = -deg * PI / 180.0;

            outH = (int)(Math.Abs(tmpW * Math.Sin(rad)) + Math.Abs(tmpH * Math.Cos(rad)));
            outW = (int)(Math.Abs(tmpH * Math.Sin(rad)) + Math.Abs(tmpW * Math.Cos(rad)));
            if (outH < tmpH)
            {
                outH = tmpH;
            }
            if (outW < tmpW)
            {
                outW = tmpW;
            }
            outImage = new byte[RGB, outH, outW];

            int new_x, new_y;
            int center_x = outW / 2, center_y = outH / 2;

            byte[,,] rot_Image = new byte[RGB, outH, outW];
            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = center_y - (tmpH / 2), a = 0; i < center_y + (tmpH / 2); i++, a++)
                {
                    for (int k = center_x - (tmpW / 2), b = 0; k < center_x + (tmpW / 2); k++, b++)
                    {
                        rot_Image[rgb, i, k] = tmpImage[rgb, a, b];
                    }
                }
            }

            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < outH; i++)
                {
                    for (int k = 0; k < outW; k++)
                    {
                        new_x = (int)((i - center_y) * Math.Sin(rad) + (k - center_x) * Math.Cos(rad) + center_x);
                        new_y = (int)((i - center_y) * Math.Cos(rad) - (k - center_x) * Math.Sin(rad) + center_y);
                        if (new_y < 0 || new_y >= outH)
                        {
                            outImage[rgb, i, k] = 0;
                        }
                        else if (new_x < 0 || new_x >= outW)
                        {
                            outImage[rgb, i, k] = 0;
                        }
                        else
                        {
                            outImage[rgb, i, k] = rot_Image[rgb, new_y, new_x];
                        }
                    }
                }
            }
            display_Image();
            saveTempFile();
            OutImageToCv();
        }
        void mosaic1_Image()
        {       // 모자이크 출력(약하게)
            if (inImage == null)
            {
                return;
            }

            outH = tmpH;
            outW = tmpW;
            outImage = new byte[RGB, outH, outW];

            int mosaic = 8;

            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < outH; i += mosaic)
                {
                    for (int k = 0; k < outW; k += mosaic)
                    {
                        if ((i + mosaic) > outH || (k + mosaic) > outW)
                        {
                            for (int a = 0; a < (outH % mosaic); a++)
                            {
                                for (int b = 0; b < (outW % mosaic); b++)
                                {
                                    outImage[rgb, i + a, k + b] = tmpImage[rgb, i + a, k + b];
                                }
                            }
                        }
                        else
                        {
                            for (int a = 0; a < mosaic; a++)
                            {
                                for (int b = 0; b < mosaic; b++)
                                {
                                    outImage[rgb, i + a, k + b] = tmpImage[rgb, i + (mosaic / 2), k + (mosaic / 2)];
                                }
                            }
                        }
                    }
                }
            }
            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = (outH - mosaic); i < outH; i++)
                {
                    for (int k = (outW - mosaic); k < outW; k++)
                    {
                        outImage[rgb, i, k] = tmpImage[rgb, i, k];
                    }
                }
            }
            display_Image();
            saveTempFile();
            OutImageToCv();
        }
        void mosaic2_Image()
        {       // 모자이크 출력(강하게)
            if (inImage == null)
            {
                return;
            }

            outH = tmpH;
            outW = tmpW;
            outImage = new byte[RGB, outH, outW];

            int mosaic = 16;

            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < outH; i += mosaic)
                {
                    for (int k = 0; k < outW; k += mosaic)
                    {
                        if ((i + mosaic) > outH || (k + mosaic) > outW)
                        {
                            for (int a = 0; a < (outH % mosaic); a++)
                            {
                                for (int b = 0; b < (outW % mosaic); b++)
                                {
                                    outImage[rgb, i + a, k + b] = tmpImage[rgb, i + a, k + b];
                                }
                            }
                        }
                        else
                        {
                            for (int a = 0; a < mosaic; a++)
                            {
                                for (int b = 0; b < mosaic; b++)
                                {
                                    outImage[rgb, i + a, k + b] = tmpImage[rgb, i + (mosaic / 2), k + (mosaic / 2)];
                                }
                            }
                        }
                    }
                }
            }
            display_Image();
            saveTempFile();
            OutImageToCv();
        }
        void emboss_Image()
        {       // 엠보싱 출력
            if (inImage == null)
            {
                return;
            }

            outH = tmpH;
            outW = tmpW;
            outImage = new byte[RGB, outH, outW];

            const int MSIZE = 3;
            double[,] mask = { { -1.0, 0.0, 0.0 }, { 0.0, 0.0, 0.0 }, { 0.0, 0.0, 1.0 } };

            double[,,] tmp_Input = new double[RGB, tmpH + 2, tmpW + 2];
            double[,,] tmp_Output = new double[RGB, outH, outW];

            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < tmpH + 2; i++)
                {
                    for (int k = 0; k < tmpW + 2; k++)
                    {
                        tmp_Input[rgb, i, k] = 127.0;
                    }
                }
            }
            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < tmpH; i++)
                {
                    for (int k = 0; k < tmpW; k++)
                    {
                        tmp_Input[rgb, i + 1, k + 1] = tmpImage[rgb, i, k];
                    }
                }
            }

            double sum = 0.0;
            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < tmpH; i++)
                {
                    for (int k = 0; k < tmpW; k++)
                    {
                        for (int a = 0; a < MSIZE; a++)
                        {
                            for (int b = 0; b < MSIZE; b++)
                            {
                                sum += tmp_Input[rgb, i + a, k + b] * mask[a, b];
                            }
                        }
                        tmp_Output[rgb, i, k] = sum + 127;
                        sum = 0.0;
                    }
                }
            }

            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < outH; i++)
                {
                    for (int k = 0; k < outW; k++)
                    {
                        if (tmp_Output[rgb, i, k] > 255)
                        {
                            tmp_Output[rgb, i, k] = 255;
                        }
                        else if (tmp_Output[rgb, i, k] < 0)
                        {
                            tmp_Output[rgb, i, k] = 0;
                        }
                        outImage[rgb, i, k] = (byte)tmp_Output[rgb, i, k];
                    }
                }
            }
            display_Image();
            saveTempFile();
            OutImageToCv();
        }
        void blurr_Image()
        {       // 블러링 출력
            if (inImage == null)
            {
                return;
            }

            outH = tmpH;
            outW = tmpW;
            outImage = new byte[RGB, outH, outW];

            const int MSIZE = 3;
            double[,] mask = { { 1 / 9.0, 1 / 9.0, 1 / 9.0 }, { 1 / 9.0, 1 / 9.0, 1 / 9.0 }, { 1 / 9.0, 1 / 9.0, 1 / 9.0 } };

            double[,,] tmp_Input = new double[RGB, tmpH + 2, tmpW + 2];
            double[,,] tmp_Output = new double[RGB, outH, outW];

            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < tmpH + 2; i++)
                {
                    for (int k = 0; k < tmpW + 2; k++)
                    {
                        tmp_Input[rgb, i, k] = 127.0;
                    }
                }
            }
            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < tmpH; i++)
                {
                    for (int k = 0; k < tmpW; k++)
                    {
                        tmp_Input[rgb, i + 1, k + 1] = tmpImage[rgb, i, k];
                    }
                }
            }

            double sum = 0.0;
            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < tmpH; i++)
                {
                    for (int k = 0; k < tmpW; k++)
                    {
                        for (int a = 0; a < MSIZE; a++)
                        {
                            for (int b = 0; b < MSIZE; b++)
                            {
                                sum += tmp_Input[rgb, i + a, k + b] * mask[a, b];
                            }
                        }
                        tmp_Output[rgb, i, k] = sum;
                        sum = 0.0;
                    }
                }
            }

            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < outH; i++)
                {
                    for (int k = 0; k < outW; k++)
                    {
                        if (tmp_Output[rgb, i, k] > 255)
                        {
                            tmp_Output[rgb, i, k] = 255;
                        }
                        else if (tmp_Output[rgb, i, k] < 0)
                        {
                            tmp_Output[rgb, i, k] = 0;
                        }
                        outImage[rgb, i, k] = (byte)tmp_Output[rgb, i, k];
                    }
                }
            }
            display_Image();
            saveTempFile();
            OutImageToCv();
        }
        void edge_Image()
        {       // 경계선 검출(sobel mask 이용)
            if (inImage == null)
            {
                return;
            }

            outH = tmpH;
            outW = tmpW;
            outImage = new byte[RGB, outH, outW];

            int[,] mask_x = new int[3, 3] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            int[,] mask_y = new int[3, 3] { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };

            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < outH; i++)
                {
                    for (int k = 0; k < outW; k++)
                    {
                        outImage[rgb, i, k] = 0;
                    }
                }
            }
            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 1; i < outH - 1; i++)
                {
                    for (int k = 1; k < outW - 1; k++)
                    {
                        float sum_x = 0, sum_y = 0;

                        for (int n = 0; n < 3; n++)
                        {
                            for (int m = 0; m < 3; m++)
                            {
                                sum_x += mask_x[n, m] * tmpImage[rgb, i + n - 1, k + m - 1];
                                sum_y += mask_y[n, m] * tmpImage[rgb, i + n - 1, k + m - 1];
                            }
                        }
                        double mag = Math.Sqrt(sum_x * sum_x + sum_y * sum_y);
                        if (mag > 255)
                        {
                            mag = 255;
                        }
                        else if (mag < 0)
                        {
                            mag = 0;
                        }
                        outImage[rgb, i, k] = (byte)mag;
                    }
                }
            }
            display_Image();
            saveTempFile();
            OutImageToCv();
        }
        void stretch_Image()
        {       // 선명하게 출력
            if (inImage == null)
            {
                return;
            }

            outH = tmpH;
            outW = tmpW;
            outImage = new byte[RGB, outH, outW];

            byte min_num = tmpImage[0, 0, 0], max_num = tmpImage[0, 0, 0];
            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < tmpH; i++)
                {
                    for (int k = 0; k < tmpW; k++)
                    {
                        if (min_num > tmpImage[rgb, i, k])
                        {
                            min_num = tmpImage[rgb, i, k];
                        }
                        if (max_num < tmpImage[rgb, i, k])
                        {
                            max_num = tmpImage[rgb, i, k];
                        }
                    }
                }
            }

            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < outH; i++)
                {
                    for (int k = 0; k < outW; k++)
                    {
                        outImage[rgb, i, k] = (byte)((double)(tmpImage[rgb, i, k] - min_num) / (max_num - min_num) * 255);
                    }
                }
            }
            display_Image();
            saveTempFile();
            OutImageToCv();
        }
        void endin_Image()
        {       // 강제로 선명하게 출력
            if (inImage == null)
            {
                return;
            }

            outH = tmpH;
            outW = tmpW;
            outImage = new byte[RGB, outH, outW];

            byte min_num = tmpImage[0, 0, 0], max_num = tmpImage[0, 0, 0];
            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < tmpH; i++)
                {
                    for (int k = 0; k < tmpW; k++)
                    {
                        if (min_num > tmpImage[rgb, i, k])
                        {
                            min_num = tmpImage[rgb, i, k];
                        }
                        if (max_num < tmpImage[rgb, i, k])
                        {
                            max_num = tmpImage[rgb, i, k];
                        }
                    }
                }
            }
            min_num += 50;
            max_num -= 50;
            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < outH; i++)
                {
                    for (int k = 0; k < outW; k++)
                    {
                        double value = (double)(tmpImage[rgb, i, k] - min_num) / (max_num - min_num) * 255;
                        if (value > 255)
                        {
                            value = 255;
                        }
                        else if (value < 0)
                        {
                            value = 0;
                        }
                        outImage[rgb, i, k] = (byte)value;
                    }
                }
            }
            display_Image();
            saveTempFile();
            OutImageToCv();
        }
        void hisequal_Image()
        {       // 평활화 출력
            if (inImage == null)
            {
                return;
            }

            outH = tmpH;
            outW = tmpW;
            outImage = new byte[RGB, outH, outW];

            for (int rgb = 0; rgb < RGB; rgb++)
            {
                long sum = 0;
                long[] hist = new long[256];
                long[] sum_hist = new long[256];
                double[] normal_hist = new double[256];
                for (int i = 0; i < 256; i++)
                {
                    hist[i] = 0;
                }
                for (int i = 0; i < tmpH; i++)
                {
                    for (int k = 0; k < tmpW; k++)
                    {
                        hist[tmpImage[rgb, i, k]]++;
                    }
                }
                for (int i = 0; i < 256; i++)
                {
                    sum += hist[i];
                    sum_hist[i] = sum;
                }
                for (int i = 0; i < 256; i++)
                {
                    normal_hist[i] = ((double)sum_hist[i] / (tmpH * tmpW)) * 255.0;
                }

                for (int i = 0; i < outH; i++)
                {
                    for (int k = 0; k < outW; k++)
                    {
                        outImage[rgb, i, k] = (byte)normal_hist[tmpImage[rgb, i, k]];
                    }
                }
            }
            display_Image();
            saveTempFile();
            OutImageToCv();
        }

        //////////////////////////////////
        //      OpepCV 함수
        //////////////////////////////////
        void bw_CvImage()
        {
            outCvImage = new Mat();

            Cv2.CvtColor(tmpCvImage, outCvImage, ColorConversionCodes.RGB2GRAY);
            Cv2.AdaptiveThreshold(outCvImage, outCvImage, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.Binary, 25, 5);

            CvToOutImage();
            display_Image();
            saveTempFile();
            OutImageToCv();
        }
        void cut_CvImage()
        {
            outCvImage = new Mat();

            int start_X = (int)getValue("자르기 시작 할 X지점을 입력하세요.", outW, 0);
            int start_Y = (int)getValue("자르기 시작 할 Y지점을 입력하세요.", outH, 0);
            int width = (int)getValue("자를 이미지의 폭을 입력하세요.", outW, 0);
            int height = (int)getValue("자를 이미지의 높이를 입력하세요.", outH, 0);
            if (width == 0 || height == 0)
            {
                return;
            }

            outCvImage = tmpCvImage.SubMat(new Rect(start_X, start_Y, width, height));

            CvToOutImage();
            display_Image();
            saveTempFile();
            OutImageToCv();
        }
        void edge_CvImage()
        {
            outCvImage = new Mat();

            tmpCvImage.CopyTo(outCvImage);
            Mat bin = new Mat();
            Mat hierarchy = new Mat();

            Cv2.CvtColor(tmpCvImage, bin, ColorConversionCodes.BGR2GRAY);
            Cv2.Threshold(bin, bin, 127, 255, ThresholdTypes.Binary);

            Cv2.FindContours(bin, out Mat[] contour, hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
            for (int i = 0; i < contour.Length; i++) { 
                Cv2.DrawContours(outCvImage, contour, i, Scalar.Blue, 3, LineTypes.AntiAlias); 
            }

            CvToOutImage();
            display_Image();
            saveTempFile();
            OutImageToCv();
        }
        void morp_CvImage()
        {
            outCvImage = new Mat();

            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Cross, new OpenCvSharp.Size(3, 3));
            Cv2.Dilate(tmpCvImage, outCvImage, kernel, new OpenCvSharp.Point(-1, -1), 3, BorderTypes.Reflect101, new Scalar(0));

            CvToOutImage();
            display_Image();
            saveTempFile();
            OutImageToCv();
        }
        void corner_CvImage()
        {
            outCvImage = new Mat();

            Mat gray = new Mat();
            outCvImage = tmpCvImage.Clone();
            Cv2.CvtColor(tmpCvImage, gray, ColorConversionCodes.BGR2GRAY);
            Point2f[] corner = Cv2.GoodFeaturesToTrack(gray, 1000, 0.03, 5, null, 3, false, 0);
            for (int i = 0; i < corner.Length; i++)
            {
                OpenCvSharp.Point pt = new OpenCvSharp.Point((int)corner[i].X, (int)corner[i].Y);
                Cv2.Circle(outCvImage, pt, 5, Scalar.Blue, Cv2.FILLED);
            }

            CvToOutImage();
            display_Image();
            saveTempFile();
            OutImageToCv();
        }
        void circle_CvImage()
        {
            Mat tmpMat = new Mat();
            tmpCvImage.CopyTo(outCvImage);

            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(3, 3));

            Cv2.CvtColor(tmpCvImage, tmpMat, ColorConversionCodes.BGR2GRAY);
            Cv2.Dilate(tmpMat, tmpMat, kernel, new OpenCvSharp.Point(-1, -1), 3);
            Cv2.GaussianBlur(tmpMat, tmpMat, new OpenCvSharp.Size(13, 13), 3, 3, BorderTypes.Reflect101);
            Cv2.Erode(tmpMat, tmpMat, kernel, new OpenCvSharp.Point(-1, -1), 3);

            CircleSegment[] circle = Cv2.HoughCircles(tmpMat, HoughModes.Gradient, 1, 100, 100, 35, 0, 0);

            for (int i = 0; i < circle.Length; i++)
            {
                OpenCvSharp.Point center = new OpenCvSharp.Point(circle[i].Center.X, circle[i].Center.Y);

                Cv2.Circle(outCvImage, center, (int)circle[i].Radius, Scalar.BlueViolet, 3);
                Cv2.Circle(outCvImage, center, 5, Scalar.BlueViolet, Cv2.FILLED);
            }

            CvToOutImage();
            display_Image();
            saveTempFile();
            OutImageToCv();
        }
        void color_CvImage()
        {
            outCvImage = new Mat();

            int low_Color = (int)getValue("검출 범위의 시작 색상을 입력하세요.", 360, 0);
            int up_Color = (int)getValue("검출 범위의 끝 색상을 입력하세요.", 360, 0);

            Mat[] mv = new Mat[3];
            Mat mask = new Mat();

            Cv2.CvtColor(tmpCvImage, tmpCvImage, ColorConversionCodes.BGR2HSV);
            mv = Cv2.Split(tmpCvImage); Cv2.CvtColor(tmpCvImage, tmpCvImage, ColorConversionCodes.HSV2BGR);
            Cv2.InRange(mv[0], new Scalar(low_Color), new Scalar(up_Color), mask);
            Cv2.BitwiseAnd(tmpCvImage, mask.CvtColor(ColorConversionCodes.GRAY2BGR), outCvImage);

            CvToOutImage();
            display_Image();
            saveTempFile();
            OutImageToCv();
        }
        void center_CvImage()
        {
            outCvImage = new Mat();

            tmpCvImage.CopyTo(outCvImage);
            Mat bin = new Mat();
            Mat hierarchy = new Mat();

            Cv2.CvtColor(tmpCvImage, bin, ColorConversionCodes.BGR2GRAY);
            Cv2.Threshold(bin, bin, 0, 255, ThresholdTypes.Otsu);

            Cv2.FindContours(bin, out Mat[] contour, hierarchy, RetrievalModes.List, ContourApproximationModes.ApproxSimple);
            for (int i = 0; i < contour.Length; i++) { 
                Moments mmt = Cv2.Moments(contour[i]);
                double cx = mmt.M10 / mmt.M00, cy = mmt.M01 / mmt.M00;
                Cv2.Circle(outCvImage, new OpenCvSharp.Point(cx, cy), 3, Scalar.Blue, -1, LineTypes.AntiAlias);
            }

            CvToOutImage();
            display_Image();
            saveTempFile();
            OutImageToCv();
        }
        void quadrangle_CvImage()
        {
            outCvImage = new Mat();

            tmpCvImage.CopyTo(outCvImage);
            Mat bin = new Mat();

            Cv2.CvtColor(tmpCvImage, bin, ColorConversionCodes.BGR2GRAY);
            Cv2.Threshold(bin, bin, 0, 255, ThresholdTypes.Otsu);

            Cv2.FindContours(bin, out OpenCvSharp.Point[][] contour, out HierarchyIndex[] hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
            for (int i = 0; i < contour.Length; i++) { 
                RotatedRect rect = Cv2.MinAreaRect(contour[i]);
                for (int k = 0; k < 4; k++) {
                    Cv2.Line(outCvImage, new OpenCvSharp.Point(rect.Points()[k].X, rect.Points()[k].Y), new OpenCvSharp.Point(rect.Points()[(k + 1) % 4].X, rect.Points()[(k + 1) % 4].Y), Scalar.Blue, 2, LineTypes.AntiAlias);
                }
            }

            CvToOutImage();
            display_Image();
            saveTempFile();
            OutImageToCv();
        }
    }
}
