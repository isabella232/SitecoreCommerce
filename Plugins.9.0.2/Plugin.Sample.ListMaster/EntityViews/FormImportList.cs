﻿
using System.Threading.Tasks;

namespace Plugin.Sample.ListMaster
{
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.EntityViews;
    using Sitecore.Commerce.Plugin.SQL;
    using Sitecore.Framework.Conditions;
    using Sitecore.Framework.Pipelines;

    /// <summary>
    /// Defines a block which populates an EntityView for a list of Coupons with the View named Public Coupons.
    /// </summary>
    /// <seealso>
    ///     <cref>
    ///         Sitecore.Framework.Pipelines.PipelineBlock{Sitecore.Commerce.EntityViews.EntityView,
    ///         Sitecore.Commerce.EntityViews.EntityView, Sitecore.Commerce.Core.CommercePipelineExecutionContext}
    ///     </cref>
    /// </seealso>
    [PipelineDisplayName("FormImportList")]
    public class FormImportList : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormImportList"/> class.
        /// </summary>
        /// <param name="commerceCommander">The <see cref="CommerceCommander"/> is a gateway object to resolving and executing other Commerce Commands and other control points.</param>
        public FormImportList(CommerceCommander commerceCommander)
        {
            this._commerceCommander = commerceCommander;
        }

        /// <summary>Runs the Command.</summary>
        /// <param name="entityView">The argument.</param>
        /// <param name="context">The context.</param>
        /// <returns>The <see cref="EntityView"/>.</returns>
        public override async Task<EntityView> Run(EntityView entityView, CommercePipelineExecutionContext context)
        {
            Condition.Requires(entityView).IsNotNull($"{this.Name}: The argument cannot be null");

            if (entityView.Name != "ListMaster_ImportList")
            {
                return entityView;
            }

            var entityViewArgument = this._commerceCommander.Command<ViewCommander>().CurrentEntityViewArgument(context.CommerceContext);

            var pluginPolicy = context.GetPolicy<PluginPolicy>();

            var listCount = await this._commerceCommander.Command<GetListCountCommand>().Process(context.CommerceContext, entityView.ItemId);


            entityView.Properties.Add(
                new ViewProperty
                {
                    Name = "Path",
                    IsHidden = false,
                    IsReadOnly = false,
                    IsRequired = true,
                    RawValue = @"C:\Users\kha\Documents\ExportedEntities"
                });

            return entityView;
        }


    }

}