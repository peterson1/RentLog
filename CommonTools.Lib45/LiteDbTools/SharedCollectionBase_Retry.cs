using CommonTools.Lib11.DTOs;
using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace CommonTools.Lib45.LiteDbTools
{
    public abstract partial class SharedCollectionBase<T>
        where T : IDocumentDTO
    {
        private static TOut Retry2x<TOut>(T record, Func<LiteCollection<T>, T, TOut> func, LiteCollection<T> coll)
        {
            try { return func(coll, record); }
            catch (IOException)
            {
                Thread.Sleep(100);
                try { return func(coll, record); }
                catch (IOException)
                {
                    Thread.Sleep(200);
                    return func(coll, record);
                }
            }
        }


        private int Retry2x(IEnumerable<T> records, Func<LiteCollection<T>, IEnumerable<T>, int> func, LiteCollection<T> coll)
        {
            try { return func(coll, records); }
            catch (IOException)
            {
                Thread.Sleep(100);
                try { return func(coll, records); }
                catch (IOException)
                {
                    Thread.Sleep(200);
                    return func(coll, records);
                }
            }
        }
    }
}
