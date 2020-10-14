using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace ReadIDCardApp
{
    /// <summary>
    /// 神思二代身份证读卡器帮助类
    /// 狗屎的神思开发文档。。。全靠猜
    /// 有问题QQ:2559791900
    /// github地址：
    /// </summary>
    public class IDCardReader
    {
        static int parg0 = 0;
        static int parg1 = 8811;
        static int parg2 = 9986;


        /// <summary>
        /// 基本函数
        /// </summary>
        /// <param name="pCmd"></param>
        /// <param name="parg0"></param>
        /// <param name="parg1"></param>
        /// <param name="parg2"></param>
        /// <returns></returns>
        [DllImport("RdCard.dll")]
        private static extern int UCommand1(ref byte pCmd, ref  int parg0, ref int parg1, ref int parg2);

        /// <summary>
        /// 初始化设备
        /// </summary>
        /// <param name="sMsg"></param>
        /// <returns></returns>
        public static bool InitCom(out string sMsg)
        {
            sMsg = string.Empty;
            int ret = 0;
            try
            {
                byte cmd = 0x41;    //初始化
                ret = UCommand1(ref cmd, ref parg0, ref parg1, ref parg2);

                if (ret == 62171)
                {
                    sMsg = "身份证读卡器连接成功";
                    return true;
                }
                else
                {
                    sMsg = "身份证读卡器连接失败!";
                }
            }
            catch (Exception ex)
            {
                sMsg = "身份证读卡器连接失败,原因是:" + ex.Message+",nret="+ ret;
            }
            return false;
        }

        /// <summary>
        /// 关闭设备
        /// </summary>
        /// <param name="sMsg"></param>
        /// <returns></returns>
        public static bool CloseCom(out string sMsg)
        {
            sMsg = string.Empty;
            try
            {
                byte cmd = 0x42;
                int nRet = UCommand1(ref cmd, ref parg0, ref parg1, ref parg2);

                if (nRet == 62171)
                {
                    sMsg = "端口关闭成功";
                    return true;
                }
                else
                    sMsg = "端口关闭失败!";
            }
            catch (Exception ex)
            {
                sMsg = "端口关闭失败,原因是:" + ex.Message;
            }

            return false;
        }

        /// <summary>
        /// 获取身份证信息
        /// </summary>
        /// <param name="sMsg"></param>
        /// <param name="sSavePath"></param>
        /// <returns></returns>
        public static CardInfo ReadCardInfo(out string sMsg, string sSavePath)
        {
            CardInfo objCardInfo = null;
            try
            {

                byte cmd = 0x43;   //验证信息
                int ret = UCommand1(ref cmd, ref parg0, ref parg1, ref parg2);    // 验证卡

                if (ret == 0)  //身份证验证成功62171
                {
                    Serilog.Log.Warning("身份证验证成功");
                    cmd = 0x49;    //读卡
                    ret = UCommand1(ref cmd, ref parg0, ref parg1, ref parg2);    // '读卡内信息
                    Serilog.Log.Warning("身份证读卡信息完成，ret={0}",ret);

                    if (ret == 62171|| ret == 62172 || ret == 62173 || ret == 62174)
                    {
                        objCardInfo = new CardInfo();
                        sMsg = string.Empty;
                        switch (ret)
                        {
                            case 62171:
                                {
                                    var idcardInfoPath = sSavePath + "wx.txt";
                                    Serilog.Log.Warning("身份证信息读取路径：" + idcardInfoPath);
                                    System.IO.StreamReader objStreamReader = new System.IO.StreamReader(sSavePath + "wx.txt", Encoding.Default);
                                    objCardInfo.Name = objStreamReader.ReadLine();
                                    objCardInfo.Sex = objStreamReader.ReadLine();
                                    objCardInfo.Nation = objStreamReader.ReadLine();
                                    objCardInfo.Birthday = objStreamReader.ReadLine();
                                    objCardInfo.Address = objStreamReader.ReadLine();
                                    objCardInfo.CardNo = objStreamReader.ReadLine();
                                    objCardInfo.Department = objStreamReader.ReadLine();
                                    objCardInfo.StartDate = objStreamReader.ReadLine();
                                    objCardInfo.EndDate = objStreamReader.ReadLine();
                                    objCardInfo.PhotoPath = sSavePath + "zp.bmp";

                                    //string sPhotoPath = objCardInfo.PhotoPath;
                                    //objCardInfo.ArrPhotoByte = ImageToByteArray(sPhotoPath);

                                    objStreamReader.Close();
                                    objStreamReader.Dispose();

                                    Serilog.Log.Warning("身份证信息为：{0}", JsonConvert.SerializeObject(objCardInfo));
                                }
                                break;
                            case 62172:
                                {
                                    var idcardInfoPath = sSavePath + "wx.txt";
                                    Serilog.Log.Warning("身份证信息读取路径："+idcardInfoPath);
                                    System.IO.StreamReader objStreamReader = new System.IO.StreamReader(sSavePath + "wx.txt", Encoding.Default);
                                    objCardInfo.Name = objStreamReader.ReadLine();
                                    objCardInfo.Sex = objStreamReader.ReadLine();
                                    objCardInfo.Nation = objStreamReader.ReadLine();
                                    objCardInfo.Birthday = objStreamReader.ReadLine();
                                    objCardInfo.Address = objStreamReader.ReadLine();
                                    objCardInfo.CardNo = objStreamReader.ReadLine();
                                    objCardInfo.Department = objStreamReader.ReadLine();
                                    objCardInfo.StartDate = objStreamReader.ReadLine();
                                    objCardInfo.EndDate = objStreamReader.ReadLine();
                                    objCardInfo.PhotoPath = sSavePath + "zp.bmp";

                                    //string sPhotoPath = objCardInfo.PhotoPath;
                                    //objCardInfo.ArrPhotoByte = ImageToByteArray(sPhotoPath);

                                    objStreamReader.Close();
                                    objStreamReader.Dispose();

                                    Serilog.Log.Warning("身份证信息为：{0}",JsonConvert.SerializeObject(objCardInfo));
                                }
                                break;
                            case 62173:
                                break;
                            case 62174:
                                break;
                        }                                           
                        return objCardInfo;
                    }
                    else if (ret == -5)
                    {
                        sMsg = "返回值：" + ret + "软件未授权!";
                    }
                    else
                    {
                        sMsg = "返回值：" + ret + "读身份证不成功";
                    }
                }
                else
                {
                    sMsg = "请将身份证放置感应区，谢谢合作！";
                }

                return objCardInfo;
            }
            catch (Exception ex)
            {
                sMsg = "读身份证失败,原因是:" + ex.Message;
                return null;
            }

        }

        /// <summary>
        /// 将图片转换成字节
        /// </summary>
        /// <param name="selectPictureFile"></param>
        /// <returns></returns>
        private static Byte[] ImageToByteArray(string selectPictureFile)
        {
            Image photo = new Bitmap(selectPictureFile);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            photo.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] imagedata = ms.GetBuffer();

            ms.Close();
            ms.Dispose();
            photo.Dispose();
            return imagedata;

        }
    }

    /// <summary>
    /// 身份证的信息
    /// </summary>
    public class CardInfo
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo;
        public int Id;
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string CardNo;
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name;
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex;
        /// <summary>
        /// 出生日期
        /// </summary>
        public string Birthday;
        /// <summary>
        /// 地址
        /// </summary>
        public string Address;
        /// <summary>
        /// 追加地址
        /// </summary>
        public string AddressEx;
        /// <summary>
        /// 发卡机关
        /// </summary>
        public string Department;
        /// <summary>
        /// 证件开始日期
        /// </summary>
        public string StartDate;
        /// <summary>
        /// 证件结束日期
        /// </summary>
        public string EndDate;
        /// <summary>
        /// 民族
        /// </summary>
        public string Nation;
        /// <summary>
        /// 分店编号
        /// </summary>
        public int ChainID;
        /// <summary>
        /// 相片路径
        /// </summary>
        public string PhotoPath;
        /// <summary>
        /// 相片的字节信息
        /// </summary>
        public byte[] ArrPhotoByte;
        /// <summary>
        /// 时时图片字节信息
        /// </summary>
        public byte[] PhoTimeByte;
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OpeTime;
        /// <summary>
        /// 操作人
        /// </summary>
        public int UserID;
        /// <summary>
        /// 操作人
        /// </summary>
        public string UserName;
        /// <summary>
        /// 入住人数
        /// </summary>
        public int nMebCount;
        /// <summary>
        /// 提交状态,1:插入成功；0：插入失败
        /// </summary>
        public string nState;
        /// <summary>
        /// 备注信息
        /// </summary>
        public string sRemark;
        /// <summary>
        /// 客户端名称
        /// </summary>
        public string ClientName;
    }
}

