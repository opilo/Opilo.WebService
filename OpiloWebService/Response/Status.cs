using System;
using System.Collections.Generic;
using System.Text;

namespace OpiloWebService.Response
{
    public class Status
    {
        /// <summary>
        /// The Message is in Opilo server queues, waiting to be sent.
        /// </summary>
        public  int QUEUED = 0;

        /// <summary>
        /// The message is sent to operator. It has operator-side-id. The operator is about to send the message to the communication co.
        /// </summary>
        public const int DELIVERED_TO_OPERATOR = 1;

        /// <summary>
        /// The message is sent to communication co. by the operator. It has operator-side-id.
        /// </summary>
        public const int DELIVERED_TO_COMMUNICATION_CO = 2;

        /// <summary>
        /// The message is delivered in the target destination.
        /// </summary>
        public const int DELIVERED_TO_DESTINATION = 3;

        /// <summary>
        /// The message was dropped while the communication co. was trying to deliver it to the target destination.
        /// The operator is not going to do any refund. The communication co. is not going to retry to send the message anymore, so the status is final.
        /// </summary>
        public const int FAILED_TO_DELIVER_TO_DESTINATION = 4;

        /// <summary>
        /// The message is dropped by Opilo while trying to send, and is refunded.
        /// </summary>
        public const int DROPPED_AND_REFUNDED = -1;

        /// <summary>
        /// The message was rejected while Opilo was trying to send it to operator. Operator did not charged the account for this message.
        /// </summary>
        public const int REJECTED_BY_OPERATOR_AND_REFUNDED = -2;

        /// <summary>
        /// The message was rejected while operator was trying to send it to communication co. and the operator has refunded it.
        /// </summary>
        public const int REJECTED_BY_COMMUNICATION_CO_AND_REFUNDED = -3;

        /// <summary>
        /// The message was dropped while the communication co. was trying to deliver it to the target destination and operator has refunded it.
        /// </summary>
        public const int REJECTED_BY_DESTINATION_AND_REFUNDED = -4;

        /// <summary>
        /// No message with this id is found.
        /// </summary>
        public const int NOT_FOUND = -5;

        /// <summary>
        /// 0,1,2,3,4,-1,-2,-3 see constants defined above
        /// </summary>
        private int code;

        public Status(int code)
        {
            this.code = code;
        }

        public int Code
        {
            get
            {
                return this.code;
            }
        }
    }
}
