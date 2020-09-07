using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ReadIDCardApp
{
    /// <summary>
    /// 神思二代读卡器Form
    /// </summary>
    public partial class IDCardReaderForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public IDCardReaderForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 读取身份证信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRead_Click(object sender, EventArgs e)
        {
            string sMsg = string.Empty;

            //初始化
            if (!IDCardReader.InitCom(out sMsg))
            {
                MessageBox.Show(sMsg);
            }

            //发布后的运行路劲，发布的是Release版本的
            string sPath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar;
            CardInfo objIDCardInfo = null;
            try
            {
                //读身份证对象
                objIDCardInfo = IDCardReader.ReadCardInfo(out sMsg, sPath);

                if (!string.IsNullOrEmpty(sMsg))
                    MessageBox.Show(sMsg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "系统提示");
            }

            if (objIDCardInfo != null)
            {
                //赋值
                txtIDCard.Text = objIDCardInfo.CardNo;
                txtName1.Text = objIDCardInfo.Name;
                txtSex1.Text = objIDCardInfo.Sex;
                txtBirthday1.Text = objIDCardInfo.Birthday;
                txtStartDate1.Text = objIDCardInfo.StartDate;
                txtEndDate1.Text = objIDCardInfo.EndDate;
                txtNation1.Text = objIDCardInfo.Nation;
                txtAddress1.Text = objIDCardInfo.Address;

                System.IO.MemoryStream objMSPhoto = new System.IO.MemoryStream(objIDCardInfo.ArrPhotoByte, 0, objIDCardInfo.ArrPhotoByte.Length);
                Image objImage = Image.FromStream(objMSPhoto);
                this.panel1.BackgroundImage = objImage;

            }
        }

        /// <summary>
        /// 清空身份证信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            txtIDCard.Text = string.Empty;
            txtName1.Text = string.Empty;
            txtSex1.Text = string.Empty;
            txtBirthday1.Text = string.Empty;
            txtStartDate1.Text = string.Empty;
            txtEndDate1.Text = string.Empty;
            txtNation1.Text = string.Empty;
            txtAddress1.Text = string.Empty;
            this.panel1.BackgroundImage = null;
        }
    }
}