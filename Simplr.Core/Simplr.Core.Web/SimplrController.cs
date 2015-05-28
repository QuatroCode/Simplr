using Raven.Client;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Simplr.Core.Web
{
    public class SimplrController : ApiController
    {
        protected readonly Lazy<IDocumentStore> LazyDocStore;

        public IDocumentStore Store => LazyDocStore.Value;

        public IAsyncDocumentSession Session { get; set; }

        public SimplrController(IDocumentStore store)
        {
            LazyDocStore = new Lazy<IDocumentStore>(() => store);
        }

        public override async Task<HttpResponseMessage> ExecuteAsync(
            HttpControllerContext controllerContext,
            CancellationToken cancellationToken)
        {
            using (Session = Store.OpenAsyncSession())
            {
                var result = await base.ExecuteAsync(controllerContext, cancellationToken);
                await Session.SaveChangesAsync();

                return result;
            }
        }
    }
}
