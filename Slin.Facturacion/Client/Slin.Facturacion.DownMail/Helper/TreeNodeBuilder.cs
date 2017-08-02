using OpenPop.Mime;
using OpenPop.Mime.Traverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CForms = System.Windows.Forms;
using Message = OpenPop.Mime.Message;

namespace Slin.Facturacion.DownMail.Helper
{
    internal class TreeNodeBuilder : IAnswerMessageTraverser<CForms.TreeNode>
    {
        public System.Windows.Forms.TreeNode VisitMessage(Message message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            // First build up the child TreeNode
            CForms.TreeNode child = VisitMessagePart(message.MessagePart);

            // Then create the topmost root node with the subject as text
            CForms.TreeNode topNode = new CForms.TreeNode(message.Headers.Subject, new[] { child });

            return topNode;
        }

        public CForms.TreeNode VisitMessagePart(MessagePart messagePart)
        {
            if (messagePart == null)
                throw new ArgumentNullException("messagePart");

            // Default is that this MessagePart TreeNode has no children
            CForms.TreeNode[] children = new CForms.TreeNode[0];

            if (messagePart.IsMultiPart)
            {
                // If the MessagePart has children, in which case it is a MultiPart, then
                // we create the child TreeNodes here
                children = new CForms.TreeNode[messagePart.MessageParts.Count];

                for (int i = 0; i < messagePart.MessageParts.Count; i++)
                {
                    children[i] = VisitMessagePart(messagePart.MessageParts[i]);
                }
            }

            // Create the current MessagePart TreeNode with the found children
            CForms.TreeNode currentNode = new CForms.TreeNode(messagePart.ContentType.MediaType, children);

            // Set the Tag attribute to point to the MessagePart.
            currentNode.Tag = messagePart;

            return currentNode;
        }
    }
}
