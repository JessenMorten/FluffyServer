using FluffyServer.Request;
using FluffyServer.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FluffyServer.Router
{
    public class HttpRouter : IHttpRouter
    {
        private readonly Regex _parameterRegex = new Regex(@"{[^/]*}");

        private readonly IList<TemplateRequestHandler> _requestHandlers = new List<TemplateRequestHandler>();

        public Func<IHttpRequest, Task<IHttpResponse>> GetHandlerOrDefault(IHttpRequest httpRequest)
        {
            string handlerKey = null;
            var routeParts = httpRequest.Route.Split("/");

            foreach (var template in _requestHandlers.Select(h => h.Template))
            {
                var templateParts = template.Split("/");

                if (templateParts.Length != routeParts.Length)
                {
                    continue;
                }

                if (template == httpRequest.Route)
                {
                    handlerKey = template;
                    break;
                }

                var isMatch = true;

                for (int i = 0; i < templateParts.Length; i++)
                {
                    var keyPart = templateParts[i];
                    var routePart = routeParts[i];

                    var isParameter = _parameterRegex.IsMatch(keyPart);

                    if (isParameter)
                    {
                        continue;
                    }
                    else if (keyPart != routePart)
                    {
                        isMatch = false;
                    }
                }

                if (isMatch)
                {
                    handlerKey = template;
                    break;
                }
            }

            if (handlerKey is not null)
            {
                var match = _requestHandlers.FirstOrDefault(h => h.Method == httpRequest.Method && h.Template == handlerKey);
                if (match is not null)
                {
                    return match.Invoker;
                }
            }

            return null;
        }

        public void On(string method, string template, Func<IHttpRequest, Task<IHttpResponse>> handler)
        {
            if (handler is not null)
            {
                _requestHandlers.Add(new TemplateRequestHandler(method, template, handler));
            }
        }

        public void On<P1>(string method, string template, Func<IHttpRequest, P1, Task<IHttpResponse>> handler)
        {
            On(method, template, async request =>
            {
                var parameters = GetParameters(request, template);
                var p1 = ToT<P1>(parameters.ElementAt(0));
                return await handler.Invoke(request, p1);
            });
        }

        public void On<P1, P2>(string method, string template, Func<IHttpRequest, P1, P2, Task<IHttpResponse>> handler)
        {
            On(method, template, async request =>
            {
                var parameters = GetParameters(request, template);
                var p1 = ToT<P1>(parameters.ElementAt(0));
                var p2 = ToT<P2>(parameters.ElementAt(1));
                return await handler.Invoke(request, p1, p2);
            });
        }

        public void On<P1, P2, P3>(string method, string template, Func<IHttpRequest, P1, P2, P3, Task<IHttpResponse>> handler)
        {
            On(method, template, async request =>
            {
                var parameters = GetParameters(request, template);
                var p1 = ToT<P1>(parameters.ElementAt(0));
                var p2 = ToT<P2>(parameters.ElementAt(1));
                var p3 = ToT<P3>(parameters.ElementAt(2));
                return await handler.Invoke(request, p1, p2, p3);
            });
        }

        private IList<string> GetParameters(IHttpRequest httpRequest, string template)
        {
            var parameters = new List<string>();
            var templateParts = template.Split("/");

            for (int i = 0; i < templateParts.Length; i++)
            {
                var templatePart = templateParts[i];

                if (_parameterRegex.IsMatch(templatePart))
                {
                    parameters.Add(httpRequest.Route.Split("/")[i]);
                }
            }

            return parameters;
        }

        private T ToT<T>(string value)
        {
            object result = default(T);

            try
            {
                if (typeof(T) == typeof(int))
                {
                    result = int.Parse(value);
                }
                else if (typeof(T) == typeof(int?))
                {
                    if (int.TryParse(value, out var i))
                    {
                        result = i;
                    }
                    else
                    {
                        result = null;
                    }
                }
                else if (typeof(T) == typeof(double?))
                {
                    if (double.TryParse(value, out var i))
                    {
                        result = i;
                    }
                    else
                    {
                        result = null;
                    }
                }
                else if (typeof(T) == typeof(double))
                {
                    result = double.Parse(value);
                }
                else if (typeof(T) == typeof(Guid?))
                {
                    if (Guid.TryParse(value, out var i))
                    {
                        result = i;
                    }
                    else
                    {
                        result = null;
                    }
                }
                else if (typeof(T) == typeof(Guid))
                {
                    result = Guid.Parse(value);
                }
                else if (typeof(T) == typeof(string))
                {
                    result = value;
                }
                else
                {
                    throw new NotSupportedException($"{typeof(T).Name} not supported");
                }
            }
            catch (Exception e)
            {

            }

            return (T)result;
        }


        private class TemplateRequestHandler
        {
            private readonly Func<IHttpRequest, Task<IHttpResponse>> _onInvoke;

            public string Method { get; init; }

            public string Template { get; init; }

            public Func<IHttpRequest, Task<IHttpResponse>> Invoker => _onInvoke;

            public TemplateRequestHandler(string method, string template, Func<IHttpRequest, Task<IHttpResponse>> onInvoke)
            {
                Method = method;
                Template = template;
                _onInvoke = onInvoke;
            }

            public async Task<IHttpResponse> Invoke(IHttpRequest httpRequest)
            {
                return await _onInvoke.Invoke(httpRequest);
            }
        }
    }
}
