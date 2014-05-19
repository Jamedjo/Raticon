using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Raticon.Control
{
    public class PlaceholderTextBehavior : Behavior<TextBox>
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(PlaceholderTextBehavior), new FrameworkPropertyMetadata(string.Empty));

        static readonly DependencyPropertyKey IsPlaceholderActivePropertyKey =
            DependencyProperty.RegisterAttachedReadOnly("IsPlaceholderActive", typeof(bool), typeof(PlaceholderTextBehavior), new FrameworkPropertyMetadata(true));

        public static readonly DependencyProperty IsPlaceholderActiveProperty = IsPlaceholderActivePropertyKey.DependencyProperty;

        public static bool GetIsPlaceholderActive(TextBox tb)
        {
            return (bool)tb.GetValue(IsPlaceholderActiveProperty);
        }

        public bool IsPlaceholderActive
        {
            get { return GetIsPlaceholderActive(AssociatedObject); }
            private set { AssociatedObject.SetValue(IsPlaceholderActivePropertyKey, value); }
        }

        public string Text
        {
            get { return (string)base.GetValue(TextProperty); }
            set { base.SetValue(TextProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.GotFocus += OnGotFocus;
            AssociatedObject.LostFocus += OnLostFocus;
            AssociatedObject.TextChanged += OnTextChanged;

            AddPlaceholderIfTextboxEmpty();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.GotFocus -= OnGotFocus;
            AssociatedObject.LostFocus -= OnLostFocus;
        }


        void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_isChangingText && !AssociatedObject.IsFocused)
            {
                AddPlaceholderIfTextboxEmpty();
            }
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            RemovePlaceholder();
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            AddPlaceholderIfTextboxEmpty();
        }

        private void RemovePlaceholder()
        {
            if (TextboxTextIsPlaceholder())
            {
                ChangeText(string.Empty);
            }
            IsPlaceholderActive = false;
        }

        private void AddPlaceholderIfTextboxEmpty()
        {
            if (string.IsNullOrEmpty(AssociatedObject.Text))
            {
                ChangeText(this.Text);
                IsPlaceholderActive = true;
            }
        }

        private bool TextboxTextIsPlaceholder()
        {
            return string.Compare(AssociatedObject.Text, this.Text, StringComparison.OrdinalIgnoreCase) == 0;
        }

        private bool _isChangingText = false;
        private void ChangeText(string newText)
        {
            _isChangingText = true;
            AssociatedObject.Text = newText;
            _isChangingText = false;
        }
    }
}