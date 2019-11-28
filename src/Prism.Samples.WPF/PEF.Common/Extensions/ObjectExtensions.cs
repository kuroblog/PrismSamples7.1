
namespace PEF.Common.Extensions
{
    using Newtonsoft.Json;

    /// <summary>
    /// 基于对象或类型的扩展方法
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// 获取对象的 Json 字符串
        /// </summary>
        /// <typeparam name="TArg"></typeparam>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static string GetJsonString<TArg>(this TArg arg) => JsonConvert.SerializeObject(arg);

        /// <summary>
        /// 将 Json 字符串转换成指定的对象
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static TObject GetJsonObject<TObject>(this string json) => JsonConvert.DeserializeObject<TObject>(json);
    }
}
