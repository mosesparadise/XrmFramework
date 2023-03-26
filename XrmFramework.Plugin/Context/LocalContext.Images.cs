using System;
using Microsoft.Xrm.Sdk;

namespace Mosesparadise.XrmFramework.Plugin.Context
{
    public partial class LocalContext
    {
        public bool HasPreImage(string imageName)
        {
            return HasImage(ExecutionContext.PreEntityImages, imageName);
            // return ExecutionContext.PreEntityImages.ContainsKey(imageName);
        }

        public virtual Entity GetPreImage(string imageName)
        {
            VerifyPreImage(imageName);
            return ExecutionContext.PreEntityImages[imageName];
        }

        public virtual Entity GetPreImageOrDefault(string imageName)
        {
            if (!ExecutionContext.PreEntityImages.ContainsKey(imageName))
            {
                return null;
            }

            return ExecutionContext.PreEntityImages[imageName];
        }

        public bool TryGetPreImage(string imageName, out Entity image)
        {
            return TryGetImage(ExecutionContext.PreEntityImages, imageName, out image);
        }

        public bool HasPostImage(string imageName)
        {
            return HasImage(ExecutionContext.PostEntityImages, imageName);
            // return ExecutionContext.PostEntityImages.ContainsKey(imageName);
        }

        public virtual Entity GetPostImage(string imageName)
        {
            VerifyPostImage(imageName);
            return ExecutionContext.PostEntityImages[imageName];
        }

        public bool TryGetPostImage(string imageName, out Entity image)
        {
            return TryGetImage(ExecutionContext.PostEntityImages, imageName, out image);
        }

        protected void VerifyPreImage(string imageName)
        {
            VerifyImage(ExecutionContext.PreEntityImages, imageName, true);
        }

        protected void VerifyPostImage(string imageName)
        {
            VerifyImage(ExecutionContext.PostEntityImages, imageName, false);
        }

        protected void VerifyImage(EntityImageCollection collection, string imageName, bool isPreImage)
        {
            if (!collection.Contains(imageName)
                || collection[imageName] == null)
            {
                throw new ArgumentNullException(imageName, $"{(isPreImage ? "PreImage" : "PostImage")} {imageName} does not exist in this context");
            }
        }

        protected bool HasImage(EntityImageCollection collection, string imageName)
        {
            return collection.Contains(imageName) && collection[imageName] != null;
        }

        protected bool TryGetImage(EntityImageCollection collection, string imageName, out Entity image)
        {
            if (HasImage(collection, imageName))
            {
                image = collection[imageName];
                return true;
            }

            image = null;
            return false;
        }
    }
}