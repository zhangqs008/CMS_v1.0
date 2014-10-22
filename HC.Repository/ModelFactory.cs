//--------------------------------------------------------------------------------
// 文件描述：实体初始化辅助类
// 文件作者：张清山
// 创建日期：2013-12-10 10:40:05
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Reflection;

namespace HC.Repository
{
    public class ModelFactory<TM> where TM : class, new()
    {
        /// <summary>
        ///     实例化并且初始化
        /// </summary>
        /// <returns></returns>
        public static TM Insten()
        {
            var m = new TM();
            var model = m as ModelBase;
            if (model != null)
            {
                model.Status = 0;
                model.IsDel = false;
                model.Sort = Convert.ToInt64(DateTime.Now.ToString("yyMMddHHmmssff", DateTimeFormatInfo.InvariantInfo));
                model.CreateDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                model.UpdateDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                model.CreateUser = "";
                model.UpdateUser = "";
            }

            return model as TM;
        }

        /// <summary>
        ///     克隆一个实体,并重新初始化固有字段
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TM Clone(TM source)
        {
            var m = new TM();
            var entity = m as ModelBase;
            if (entity != null)
            {
                Type type = source.GetType();
                PropertyInfo[] properties = type.GetProperties();

                foreach (PropertyInfo p in properties)
                {
                    object value = p.GetValue(source, null);
                    p.SetValue(entity, value, null);
                }

                entity.Status = 0;
                entity.IsDel = false;
                entity.Sort = Convert.ToInt64(DateTime.Now.ToString("yyMMddHHmmssff", DateTimeFormatInfo.InvariantInfo));
                entity.CreateDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                entity.UpdateDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                entity.CreateUser = "";
                entity.UpdateUser = "";
            }
            return entity as TM;
        }


        /// <summary>
        ///     克隆一个表单实体,并重新初始化固有字段
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TM CloneForm(TM source)
        {
            var m = new TM();
            var entity = m as ModelBase;
            if (entity != null)
            {
                Type type = source.GetType();
                PropertyInfo[] properties = type.GetProperties();

                foreach (PropertyInfo p in properties)
                {
                    object value = p.GetValue(source, null);
                    p.SetValue(entity, value, null);
                }

                entity.Status = 0;
                entity.IsDel = false;
                entity.UpdateDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                entity.CreateUser = "";
                entity.UpdateUser = "";
            }
            return entity as TM;
        }
    }
}