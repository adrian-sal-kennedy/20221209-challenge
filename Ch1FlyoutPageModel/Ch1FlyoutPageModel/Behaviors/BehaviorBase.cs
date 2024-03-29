namespace Ch1FlyoutPageModel.Behaviors
{
    using System;
    using System.Collections.Generic;
    using ViewModels;
    using Xamarin.Forms;

    public class BehaviorBase<T> : Behavior<T> where T : BindableObject
    {
        public T? AssociatedObject { get; private set; }


        protected override void OnAttachedTo(T bindable)
        {
            base.OnAttachedTo(bindable);
            AssociatedObject = bindable;

            if (bindable.BindingContext != null)
            {
                BindingContext = bindable.BindingContext;
            }

            bindable.BindingContextChanged += OnBindingContextChanged;
        }

        protected override void OnDetachingFrom(T bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.BindingContextChanged -= OnBindingContextChanged;
            AssociatedObject = null;
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();

            if (sender is not VisualElement visualElement) { return; }

            if (visualElement.BindingContext == null)
            {
                OnDetachingFrom(visualElement);
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject?.BindingContext;
        }
    }
}
