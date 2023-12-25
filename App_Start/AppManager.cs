using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using SenseNet.ContentRepository.Storage;

namespace StudentLinkUp_MVC_
{
    public static class SignInManager
    {
        public static Queue signedIn = new Queue();

        public static void Empty()
        {
            signedIn.Clear();
        }

        public static bool isEmpty()
        {
            if (signedIn.Count == 0)
                return true;
            else
                return false;
        }
    }

    public static class MeetingsManager
    {
        public static Queue meetingParticipants = new Queue();
        public static Queue materials = new Queue();
        public static bool closed = false;

        public static void Empty()
        {
            meetingParticipants.Clear();
            materials.Clear();
        }

        public static bool isEmpty()
        {
            if (meetingParticipants.Count == 0)
                return true;
            else
                return false;
        }

        public static void closeSale()
        {
            closed = true;
        }

        public static void AddParticipant(UserProfile user)
        {
            meetingParticipants.Enqueue(user);
        }

        public static void AddMaterial(LearningMaterial item)
        {
            materials.Enqueue(item);
        }

        public static UserProfile GetParticipant()
        {
            return (UserProfile)meetingParticipants.Dequeue();
        }

        public static LearningMaterial GetItem()
        {
            return (LearningMaterial)materials.Dequeue();
        }
    }

    public static class ReceiptsManager
    {
        public static Queue<Receipt> receipts = new Queue<Receipt>();
        public static Queue<LearningMaterial> pendingMaterials = new Queue<LearningMaterial>();
        
        public static void Empty()
        {
            receipts.Clear();
            pendingMaterials.Clear();
        }

        public static bool receiptsIsEmpty()
        {
            if (receipts.Count == 0)
                return true;
            else
                return false;
        }

        public static bool pendingMaterialsIsEmpty()
        {
            if (pendingMaterials.Count == 0)
                return true;
            else
                return
                    false;
        }

        public static void AddReceipt(Receipt receipt)
        {
            receipts.Enqueue(receipt);
        }

        public static void AddPendingMaterial(LearningMaterial material)
        {
            pendingMaterials.Enqueue(material);
        }

        public static Receipt GetReceipt()
        {
            return receipts.Dequeue();
        }

        public static LearningMaterial GetPendingMaterial()
        {
            return pendingMaterials.Dequeue();
        }
    }
}