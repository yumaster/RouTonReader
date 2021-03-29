using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace JingLunReader
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 获取身份证信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ReadCard_Click(object sender, EventArgs e)
        {
            ClearData();
            byte[] pMsg = ReaderApi.ReadIDCardInfo();
            if (pMsg.Length < 1)
                return;

            txtName.Text = GetStrByBytes(pMsg, 0, 31);
            txtSex.Text = GetStrByBytes(pMsg, 31, 3);
            txtNation.Text = GetStrByBytes(pMsg, 34, 10);
            txtBirth.Text = GetStrByBytes(pMsg, 44, 9);
            txtAddress.Text = GetStrByBytes(pMsg, 53, 71);
            txtIdCard.Text = GetStrByBytes(pMsg, 124, 19);
            txtFrom.Text = GetStrByBytes(pMsg, 143, 31);
            StringBuilder sb = new StringBuilder(GetStrByBytes(pMsg, 174, 9));
            txtValidTo.Text = sb.Append(" - " + GetStrByBytes(pMsg, 183, 9)).ToString();
            p_img.BackgroundImage = Image.FromFile("photo.bmp");
        }

        /// <summary>
        /// 获取身份证内码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ReadSn_Click(object sender, EventArgs e)
        {
            txtSn.Text = "";
            txtSn.Text = GetSnByType("ID");
        }
        /// <summary>
        /// 获取IC卡内码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ICCardSn_Click(object sender, EventArgs e)
        {
            txtSn.Text = "";
            txtSn.Text = GetSnByType("IC");
        }

        /// <summary>
        /// 根据字节获取字符串
        /// </summary>
        /// <param name="pMsg"></param>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private string GetStrByBytes(byte[] pMsg, int start, int size)
        {
            byte[] bs = new byte[size];
            Array.Copy(pMsg, start, bs, 0, size);
            return System.Text.Encoding.Default.GetString(bs).Trim().Replace("\0", "");
        }
        /// <summary>
        /// 根据卡类型，获取卡内码
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetSnByType(string type)
        {
            byte[] bs = new byte[] { };
            StringBuilder sb = new StringBuilder();
            if (type=="ID")//ID身份证
            {
                bs = ReaderApi.ReadIDCardSn();
                if (bs.Length < 1)
                    return "";
            }else
            {
                bs = ReaderApi.ReadICCardSn();
            }
            foreach (byte item in bs)
            {
                char c = (char)item;
                sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        private void ClearData()
        {
            txtName.Text = "";
            txtSex.Text = "";
            txtNation.Text = "";
            txtBirth.Text = "";
            txtAddress.Text = "";
            txtIdCard.Text = "";
            txtFrom.Text = "";
            txtValidTo.Text = "";
            p_img.BackgroundImage = null;
        }

        private void btn_Read_Click(object sender, EventArgs e)
        {
            string snStr = string.Empty;
            txtSn.Text = snStr;

            snStr = GetSnByType("IC");//IC卡序列号
            if(string.IsNullOrEmpty(snStr))
            {
                byte[] pMsg = ReaderApi.ReadIDCardInfo();
                if (pMsg.Length >= 1)
                {
                    snStr = GetStrByBytes(pMsg, 124, 19);
                }
            }

            txtSn.Text = snStr;
        }
    }
}
