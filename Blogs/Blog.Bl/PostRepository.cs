using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsAzure.Table;
using Blog.Core;
using log4net;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Blog.Bl
{
    public class PostRepository
    {
        private ILog _log = LogManager.GetLogger(typeof(PostRepository));
        private static readonly CloudStorageAccount Storage = CloudStorageAccount.DevelopmentStorageAccount;
        private static readonly CloudTableClient Client = Storage.CreateCloudTableClient();


        public List<Post> GetPosts()
        {
            try
            {
                var post = new TableSet<Post>(Client);
                var tmp = post.ToList();
                return tmp.Any() ? tmp.ToList() : null;
            }
            catch (Exception exception)
            {
                _log.Error(exception);
            }
            return null;
        }

        public Post GetPost(string title)
        {
            try
            {
                var post = new TableSet<Post>(Client);
                var tmp = post.FirstOrDefault(p => p.PostTitle == title);

                return tmp ?? null;
            }
            catch (Exception exception)
            {
                _log.Error(exception);
            }
            return null;
        }

        public void InsertPost(Post _post)
        {
            try
            {
                _post.PostDateTime = DateTime.Now;
                var post = new TableSet<Post>(Client);
                post.Add(_post);
            }
            catch (Exception exception)
            {
                _log.Error(exception);
            }

        }
    }
}
