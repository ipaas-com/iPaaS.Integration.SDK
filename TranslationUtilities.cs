using Integration.Abstract.Helpers;
using Integration.Abstract.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Abstract
{
    public abstract class TranslationUtilities
    {
        //public  Connection ActiveConnection;

        //Given a mapping collection type and a source object, return the Id of the source object. This is used for ExternalId lookups and saves
        //For example, if we get an iPaaS product, we need to return the Id. If we get a Counterpoint Item, we need to return the ITEM_NO
        //The mappingCollectionId parameter is optional and generally unnecessary for external systems. There are a handful of instances where
        //we have a parent-only mapping that comes up in multiple mapping collections with no other way to distinguish IDs. For example, we have 5 different
        //parent only mappings to Product Units from one system to iPaas. Since there's no way to know from this function which unit they are looking at, we have to
        //have something unique to distinguish the unit. 
        public abstract string GetPrimaryId(Connection connection, int mappingCollectionType, object sourceObject, long? mappingCollectionId = null);

        //Given an object, it’s mapping collection type, and it’s primary ID, assign the primary ID to the object. For example, if we pass a customer
        //  object and the customer's ID, this method should assign the customer ID to appropriate field on the customer object.
        public abstract void SetPrimaryId(Connection connection, int mappingCollectionType, object sourceObject, string primaryId);

        //Given a mapping collection type, return a blank instance of the specified type.
        public abstract object GetDestinationObject(Connection connection, int mappingCollectionType);

        //Given a mapping collection type and a unique identifier, return a populated object of that type for the id given.
        //For example, if a Spaceport connection is given the type PRODUCT and the Id 7, we will need to return a ProductResponse for Id 7

        /// <summary>
        /// Get an object by id and mapping collection type
        /// </summary>
        /// <param name="mappingCollectionType">The mapping collection type id</param>
        /// <param name="id">The id of the object</param>
        /// <returns></returns>
        public abstract Task<ResponseObject> ModelGetAsync(Connection connection, int mappingCollectionType, object id);

        /// <summary>
        /// Create a new object
        /// </summary>
        /// <param name="mappingCollectionType">The mapping collection type id</param>
        /// <param name="sourceObject">The object being created</param>
        /// <param name="id">The id of the object</param>
        /// <returns></returns>
        public abstract Task<ResponseObject> ModelCreateAsync(Connection connection, int mappingCollectionType, object sourceObject, object id, CollisionHandlerSettings collisionHandlerSettings);

        /// <summary>
        /// Updates an existing object
        /// </summary>
        /// <param name="mappingCollectionType">The mapping collection type id</param>
        /// <param name="sourceObject">The object being modified</param>
        /// <param name="id">The id of the object</param>
        /// <returns></returns>
        public abstract Task<ResponseObject> ModelUpdateAsync(Connection connection, int mappingCollectionType, object sourceObject, object id, CollisionHandlerSettings collisionHandlerSettings);

        /// <summary>
        /// Delete an object by its id
        /// </summary>
        /// <param name="MappingCollectionType">The mapping collection type id</param>
        /// <param name="Id">The id of the object</param>
        public abstract Task<ResponseObject> ModelDeleteAsync(Connection connection, int mappingCollectionType, object id);

        /// <summary>
        /// Validate the connection (usually by making a call to a pre-determined endpoint)
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public abstract Task<string> ValidateConnection(Connection connection);

        /// <summary>
        /// Creates a list of new external ids to save
        /// </summary>
        /// <remarks>
        /// Some objects may require additional handling for ExternalIds. For example, if we save a product with variants to BigCommerce, we need to save the
        /// external ids from the child objects as well. We call this function to create a list of new external ids to save
        /// </remarks>
        /// <param name="mappingCollectionType"></param>
        /// <param name="sourceObject"></param>
        /// <param name="destinationObject"></param>
        /// <returns></returns>
        public abstract List<TranslationExternalId> CollectAdditionalExternalIds(Connection connection, int mappingCollectionType, object sourceObject, object destinationObject);

        /// <summary>
        /// Returns a list of subtable mappings
        /// </summary>
        /// <remarks>
        /// Given a type, mappingCollectionId, and direction, return a list of subtable mappings. The SubTableMapping class includes source and destination info
        /// </remarks>
        /// <param name="mappingCollectionType"></param>
        /// <param name="mappingResponseId"></param>
        /// <param name="mappingDirection"></param>
        /// <returns></returns>
        public abstract List<ChildMapping> GetChildMappings(Connection connection, int mappingCollectionType, long mappingResponseId, int mappingDirection);

        /// <summary>
        /// Given a mapping collection type, transfer any initial data requirements. This may include data going both to and from iPaaS. For example, your CRM 
        ///     software might initialize customer data to iPaaS, while your website initializes product categories from iPaaS.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="mappingCollectionType"></param>
        /// <returns></returns>
        public abstract Task InitializeData(Connection connection, int mappingCollectionType);

        /// <summary>
        /// Given a data transfer request, determine if there is anything we need to verify or transfer first. For example, if we are running a transaction 
        ///     transfer that includes a gift card payment, we may want to validate that the gift card exists in the target system and transfer it if it is not.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transferRequest"></param>
        /// <returns></returns>
        public abstract Task<object> HandlePrerequisite(Connection connection, TransferRequest transferRequest);

        /// <summary>
        /// Given a data transfer request, determine if there is anything we need to do after the transfer is complete. For example, after we transfer an 
        ///     order, we may want to transfer any associated deposit tickets too.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transferRequest"></param>
        /// <returns></returns>
        public abstract Task<object> HandlePostActions(Connection connection, TransferRequest transferRequest);

        /// <summary>
        /// Updates webhook subscriptions in the external system async
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="subscribed"></param>
        /// <returns></returns>
        public abstract Task<ResponseObject> UpdateWebhookSubscriptionAsync(Connection connection, string scope, bool subscribed);

        /// <summary>
        /// An optionally overridable function that allows the integration to return a list of transfer request objects in response
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="mappingCollectionType"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<List<BulkTransferRequest>> BulkTransfer(Connection connection, int mappingCollectionType, string filter)
        {
            return null;
        }


        /// <summary>
        /// This is an optionally overridable function that allows us to estimate the number of external API calls that a given transfer will require.
        ///     The value returned here is used to claim an estimated amount of API calls on systems that are throttled. If this feature is not implemented
        ///     bursty transfer requests may overload the external system.
        /// Note that this function is only called on transfers FROM iPaaS. On transfers going TO iPaaS, the source data will be pulled early in the transfer
        ///     process, so the number of API calls required will be a known quantity already.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="mappingCollectionType"></param>
        /// <param name="sourceObject"></param>
        /// <returns>The estimated number of external API calls that will be made during a transfer. The default return value is 1.</returns>
        public long EstimateTotalAPICallsMade(Connection connection, int mappingCollectionType, object sourceObject, int direction)
        {
            return 1;
        }

        /// <summary>
        /// This is an optionally overridable function that allows us to process authorization data (e.g. convert temporary OAuth values into a 
        ///     set of permanent keys. 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="authorization"></param>
        public void ProcessAuthorization(Connection connection, Authorization authorization)
        {
            ;
        }
    }
}
