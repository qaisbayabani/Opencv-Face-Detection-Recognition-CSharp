using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using System.Drawing;
using OpenCvSharp.Extensions;
using System.Threading;
using Point = OpenCvSharp.Point;
using OpenCvSharp.Face;

using System.IO;
using System.Linq.Expressions;

namespace Try
{

    class Program
    {
        //static VideoCapture cap = new VideoCapture("rtsp://192.168.0.5:8554/profile1",
        //  VideoCaptureAPIs.FFMPEG);

        //(laptopcam)static VideoCapture cap = new VideoCapture(0);
        //(camcare)"rtsp://192.168.10.3:8554/profile0"
        //(imou)rtsp://admin:a12345678@192.168.1.2:554/cam/realmonitor?channel=1&subtype=0
        //(digitus)rtsp://192.168.1.51:554/ch0_0.h264
        //rtsp://192.168.10.3:554/ch0_1.h264



        //static VideoCapture cap = new VideoCapture("rtsp://192.168.10.3:554/ch0_1.h264");

        static VideoCapture cap = new VideoCapture(0);



        static CascadeClassifier haarCascade = new CascadeClassifier("haarcascade_frontalface_alt.xml");
        
        static Rect[] faces;
        static Rect[] faces2;

        static Mat src = new Mat();
        static Mat gray = new Mat();
        private static int sleepTime = 0;// (int)Math.Round(1000 / cap.Fps);

        private static EigenFaceRecognizer recognizer;
        private static List<Mat> TrainedFaces = new List<Mat>();
        private static List<int> PersonsLabes = new List<int>();
        private static List<string> PersonsNames = new List<string>();
        private static string sst = "*";
        private static OpenCvSharp.Size si = new OpenCvSharp.Size(300, 300);
        private static Rect face2;
        private static int xx, yy;

        public static void Main()
        {

            TrainedFaces.Clear();
            PersonsLabes.Clear();
            PersonsNames.Clear();
            PersonsNames.Add("Scanning============>or press s to add face");

            int ImagesCount = 0;

            string path = Directory.GetCurrentDirectory();
            string[] files = Directory.GetFiles(path, "*.png", SearchOption.AllDirectories);


            foreach (var file in files)
            {


                Mat m = new Mat(file, ImreadModes.Grayscale);

                Cv2.Resize(m, m, si);

                string excl = file.Substring(26, 6);

                TrainedFaces.Add(m);

                PersonsLabes.Add(ImagesCount);

                PersonsNames.Add(excl);

                ImagesCount++;

            }////FACES FOR EACH

            Console.WriteLine(ImagesCount + "======Images in Database======press s to add faces");

            recognizer = EigenFaceRecognizer.Create(ImagesCount, 2000);

            //if (TrainedFaces[0] != null)
            //{

            recognizer.Train(TrainedFaces.ToArray(), PersonsLabes.ToArray());

            Ok1 ok1 = new Ok1();

            Thread ti = new Thread(new ThreadStart(() => ok1.Ok()));
            ti.Start();


        }

        class Ok1
        {

            public void Ok()
            {

                Func1 func = new Func1();
                Func2 func2 = new Func2();

                while (true)
                {
                
                    cap.Read(src);

                    Window win3 = new Window("Croppic", WindowMode.KeepRatio);

                    Window win4 = new Window("Main", WindowMode.KeepRatio);

                    if (!src.Empty())
                    {
                        sleepTime = (int)Math.Round(1000 / cap.Fps);

                        Cv2.WaitKey(1);

                        //Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);


                        DateTime dt = DateTime.Now;
                        
                        yy = xx;

                        xx = dt.Second;


                        //Console.WriteLine(xx);


                        if (xx > yy)
                        {
                         

                            Thread ti = new Thread(new ThreadStart(() => func.func1()));
                            ti.Start();


                        }


                        if (Console.KeyAvailable)
                        {
                        
                            if (Console.ReadKey().Key == ConsoleKey.S)
                            {

                                func2.func2();
               
                            }



                        }///console key pressed



                    }///while not src empty


                        Cv2.PutText(src, sst, face2.TopLeft, HersheyFonts.HersheyPlain, 1, new Scalar(100, 200, 60), 1);
                    try
                    {
                        win3.ShowImage(src);

                        if (!gray.Empty())
                        {

                            win4.ShowImage(gray);
                        }

                    }
                    catch (Exception e) { 
                    
                    }
                }//while true

            }///public void ok




            class Func1
            {
                public void func1()
                {

                    try
                    {

                            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

                            faces = haarCascade.DetectMultiScale(
                                  gray, 1.1, 3, HaarDetectionType.ScaleImage);

                    
                    }
                    catch(Exception e) { }

                    if (faces != null)
                        foreach (var face in faces)
                        {

                            try
                            {
                                Point p = new Point(face.Left, face.Top);
                                Point pp = new Point(face.Right, face.Bottom);

                                src.Rectangle(p, pp, new Scalar(220, 100, 255), 1);

                                //src.DrawMarker(p, new Scalar(20, 2, 100), MarkerTypes.Cross, 250, 1);
                                //byte[] b=src.ImEncode("1.png");
                                gray = src.SubMat(face.Top, face.Bottom, face.Left, face.Right);
                                
                                Cv2.CvtColor(gray, gray, ColorConversionCodes.BGR2GRAY);

                                Cv2.Resize(gray, gray, si);

                                int x = recognizer.Predict(gray);////////////////////////////////

                                if (x < 100)
                                {
                                    //   Thread.Sleep(2000);

                                    Console.WriteLine("Found=========="+ PersonsNames[x]);

                                    sst = PersonsNames[x];
                                    
                                    x = 0;

                                }

                                else
                                {

                                    Console.WriteLine("Scanning.......................");

                                }

                                //}////if trained faces !=null

                                face2 = face;
                            }
                            catch (Exception e) { 
                            
                            }
                        }///var face in faces



                }///END OF FUNC1 FUNCTION
            }//END OF CLASS FUNC1




            class Func2
            {
                public void func2()
                {

                    Console.WriteLine("Please enter the name========>");
                    string name = Console.ReadLine();

                    for (int i = 0; i < 10; i++)
                    {
                        cap.Read(src);
                        Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

                        try
                        {
                            faces = haarCascade.DetectMultiScale(
                                  gray, 1.1, 3, HaarDetectionType.ScaleImage);
                        }
                        catch (Exception e)
                        {

                        }
                        if (faces != null)
                            foreach (var face in faces)
                            {

                                Point p = new Point(face.Left, face.Top);
                                Point pp = new Point(face.Right, face.Bottom);
                                src.Rectangle(p, pp, new Scalar(220, 100, 255), 1);
                                //src.DrawMarker(p, new Scalar(20, 2, 100), MarkerTypes.Cross, 250, 1);
                                //byte[] b=src.ImEncode("1.png");
                                gray = src.SubMat(face.Top, face.Bottom, face.Left, face.Right);
                                Cv2.CvtColor(gray, gray, ColorConversionCodes.BGR2GRAY);

                                Cv2.Resize(gray, gray, si);


                                gray.SaveImage(name + i.ToString() + ".png");


                            }///face in faces

                    }////for LOOP

                }///END OF FUNC2

            }//end of class func2

        }///class ok1




    }///class program
}//namespace try


