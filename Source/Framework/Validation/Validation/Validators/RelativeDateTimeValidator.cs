//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright ?Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Validators
{
	/// <summary>
	/// Validates a <see cref="DateTime"/> value by checking it belongs to a range relative to the current date.
	/// </summary>
	[ConfigurationElementType(typeof(RelativeDateTimeValidatorData))]
	public class RelativeDateTimeValidator : ValueValidator<DateTime>
	{
		private RangeChecker<DateTime> rangeChecker;
		private int lowerBound;
		private DateTimeUnit lowerUnit;
		private int upperBound;
		private DateTimeUnit upperUnit;
		private RelativeDateTimeGenerator generator;

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidator"/>.</para>
		/// </summary>
		/// <param name="upperBound">The upper bound</param>
		/// <param name="upperUnit">The upper bound unit of time.</param>
		public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit)
			: this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, RangeBoundaryType.Inclusive, false)
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidator"/>.</para>
		/// </summary>
		/// <param name="upperBound">The upper bound</param>
		/// <param name="upperUnit">The upper bound unit of time.</param>
		/// <param name="negated">True if the validator must negate the result of the validation.</param>
		public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit, bool negated)
			: this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, RangeBoundaryType.Inclusive, negated)
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidator"/>.</para>
		/// </summary>
		/// <param name="upperBound">The upper bound</param>
		/// <param name="upperUnit">The upper bound unit of time.</param>
		/// <param name="messageTemplate">The message template to use when logging results.</param>
		public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit, string messageTemplate)
			: this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, RangeBoundaryType.Inclusive, messageTemplate)
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidator"/>.</para>
		/// </summary>
		/// <param name="upperBound">The upper bound</param>
		/// <param name="upperUnit">The upper bound unit of time.</param>
		/// <param name="messageTemplate">The message template to use when logging results.</param>
		/// <param name="negated">True if the validator must negate the result of the validation.</param>
		public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit, string messageTemplate, bool negated)
			: this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, RangeBoundaryType.Inclusive, messageTemplate, negated)
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidator"/>.</para>
		/// </summary>
		/// <param name="upperBound">The upper bound</param>
		/// <param name="upperUnit">The upper bound unit of time.</param>
		/// <param name="upperBoundType">The indication of how to perform the upper bound check.</param>
		public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType)
			: this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, upperBoundType, false)
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidator"/>.</para>
		/// </summary>
		/// <param name="upperBound">The upper bound</param>
		/// <param name="upperUnit">The upper bound unit of time.</param>
		/// <param name="upperBoundType">The indication of how to perform the upper bound check.</param>
		/// <param name="negated">True if the validator must negate the result of the validation.</param>
		public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType, bool negated)
			: this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, upperBoundType, negated)
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidator"/>.</para>
		/// </summary>
		/// <param name="upperBound">The upper bound</param>
		/// <param name="upperUnit">The upper bound unit of time.</param>
		/// <param name="upperBoundType">The indication of how to perform the upper bound check.</param>
		/// <param name="messageTemplate">The message template to use when logging results.</param>
		public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType, string messageTemplate)
			: this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, upperBoundType, messageTemplate)
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidator"/>.</para>
		/// </summary>
		/// <param name="upperBound">The upper bound</param>
		/// <param name="upperUnit">The upper bound unit of time.</param>
		/// <param name="upperBoundType">The indication of how to perform the upper bound check.</param>
		/// <param name="messageTemplate">The message template to use when logging results.</param>
		/// <param name="negated">True if the validator must negate the result of the validation.</param>
		public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType, string messageTemplate, bool negated)
			: this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, upperBoundType, messageTemplate, negated)
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidator"/>.</para>
		/// </summary>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="lowerUnit">The lower bound unit of time.</param>
		/// <param name="upperBound">The upper bound</param>
		/// <param name="upperUnit">The upper bound unit of time.</param>
		public RelativeDateTimeValidator(int lowerBound, DateTimeUnit lowerUnit, int upperBound, DateTimeUnit upperUnit)
			: this(lowerBound, lowerUnit, RangeBoundaryType.Inclusive, upperBound, upperUnit, RangeBoundaryType.Inclusive, false)
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidator"/>.</para>
		/// </summary>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="lowerUnit">The lower bound unit of time.</param>
		/// <param name="upperBound">The upper bound</param>
		/// <param name="upperUnit">The upper bound unit of time.</param>
		/// <param name="negated">True if the validator must negate the result of the validation.</param>
		public RelativeDateTimeValidator(int lowerBound, DateTimeUnit lowerUnit, int upperBound, DateTimeUnit upperUnit, bool negated)
			: this(lowerBound, lowerUnit, RangeBoundaryType.Inclusive, upperBound, upperUnit, RangeBoundaryType.Inclusive, negated)
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidator"/>.</para>
		/// </summary>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="lowerUnit">The lower bound unit of time.</param>
		/// <param name="lowerBoundType">The indication of how to perform the lower bound check.</param>
		/// <param name="upperBound">The upper bound</param>
		/// <param name="upperUnit">The upper bound unit of time.</param>
		/// <param name="upperBoundType">The indication of how to perform the upper bound check.</param>
		public RelativeDateTimeValidator(int lowerBound, DateTimeUnit lowerUnit, RangeBoundaryType lowerBoundType,
			int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType)
			: this(lowerBound, lowerUnit, lowerBoundType, upperBound, upperUnit, upperBoundType, false)
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidator"/>.</para>
		/// </summary>
		/// <param name="negated">True if the validator must negate the result of the validation.</param>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="lowerUnit">The lower bound unit of time.</param>
		/// <param name="lowerBoundType">The indication of how to perform the lower bound check.</param>
		/// <param name="upperBound">The upper bound</param>
		/// <param name="upperUnit">The upper bound unit of time.</param>
		/// <param name="upperBoundType">The indication of how to perform the upper bound check.</param>
		public RelativeDateTimeValidator(int lowerBound, DateTimeUnit lowerUnit, RangeBoundaryType lowerBoundType,
			int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType,
			bool negated)
			: this(lowerBound, lowerUnit, lowerBoundType, upperBound, upperUnit, upperBoundType, null, negated)
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidator"/>.</para>
		/// </summary>
		/// <param name="messageTemplate">The message template to use when logging results.</param>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="lowerUnit">The lower bound unit of time.</param>
		/// <param name="lowerBoundType">The indication of how to perform the lower bound check.</param>
		/// <param name="upperBound">The upper bound</param>
		/// <param name="upperUnit">The upper bound unit of time.</param>
		/// <param name="upperBoundType">The indication of how to perform the upper bound check.</param>
		public RelativeDateTimeValidator(int lowerBound, DateTimeUnit lowerUnit, RangeBoundaryType lowerBoundType,
			int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType,
			string messageTemplate)
			: this(lowerBound, lowerUnit, lowerBoundType, upperBound, upperUnit, upperBoundType, messageTemplate, false)
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RelativeDateTimeValidator"/>.</para>
		/// </summary>
		/// <param name="negated">True if the validator must negate the result of the validation.</param>
		/// <param name="messageTemplate">The message template to use when logging results.</param>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="lowerUnit">The lower bound unit of time.</param>
		/// <param name="lowerBoundType">The indication of how to perform the lower bound check.</param>
		/// <param name="upperBound">The upper bound</param>
		/// <param name="upperUnit">The upper bound unit of time.</param>
		/// <param name="upperBoundType">The indication of how to perform the upper bound check.</param>
		public RelativeDateTimeValidator(int lowerBound, DateTimeUnit lowerUnit, RangeBoundaryType lowerBoundType,
			int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType,
			string messageTemplate, bool negated)
			: base(messageTemplate, null, negated)
		{
			ValidatorArgumentsValidatorHelper.ValidateRelativeDatimeValidator(lowerBound, lowerUnit, lowerBoundType, upperBound, upperUnit, upperBoundType);

			this.lowerBound = lowerBound;
			this.lowerUnit = lowerUnit;
			this.upperBound = upperBound;
			this.upperUnit = upperUnit;

			generator = new RelativeDateTimeGenerator();
			DateTime nowDateTime = DateTime.Now;
			DateTime lowerDateTime = generator.GenerateBoundDateTime(lowerBound, lowerUnit, nowDateTime);
			DateTime upperDateTime = generator.GenerateBoundDateTime(upperBound, upperUnit, nowDateTime);
			this.rangeChecker = new RangeChecker<DateTime>(lowerDateTime, lowerBoundType, upperDateTime, upperBoundType);
		}

		/// <summary>
		/// Implements the validation logic for the receiver.
		/// </summary>
		/// <param name="objectToValidate">The object to validate.</param>
		/// <param name="currentTarget">The object on the behalf of which the validation is performed.</param>
		/// <param name="key">The key that identifies the source of <paramref name="objectToValidate"/>.</param>
		/// <param name="validationResults">The validation results to which the outcome of the validation should be stored.</param>
		/// <remarks>
		/// The implementation for this method will perform type checking and converstion before forwarding the 
		/// validation request to method <see cref="Validator{T}.DoValidate(T, object, string, ValidationResults)"/>.
		/// </remarks>
		/// <see cref="Validator.DoValidate"/>
		protected internal override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			if (objectToValidate == null)
			{
				this.LogValidationResult(validationResults,
					GetMessage(objectToValidate, key),
					currentTarget,
					key);
			}
			else
			{
				base.DoValidate(objectToValidate, currentTarget, key, validationResults);
			}
		}

		/// <summary>
		/// Implements the validation logic for the receiver.
		/// </summary>
		/// <param name="objectToValidate">The object to validate.</param>
		/// <param name="currentTarget">The object on the behalf of which the validation is performed.</param>
		/// <param name="key">The key that identifies the source of <paramref name="objectToValidate"/>.</param>
		/// <param name="validationResults">The validation results to which the outcome of the validation should be stored.</param>
		protected override void DoValidate(DateTime objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
            DateTime result ;

            switch (this.upperUnit)
            {
                case DateTimeUnit.Day:
                case DateTimeUnit.Month: 
                case DateTimeUnit.Year: 
                    result = objectToValidate.Date;
                    break;
                //case DateTimeUnit.Hour:
                //case DateTimeUnit.Minute: 
                //case DateTimeUnit.Second:
                //    result = objectToValidate;
                //    break;
                default:
                    result = objectToValidate;
                    break;
            }

            if (this.rangeChecker.IsInRange(result) == Negated)
			{
				this.LogValidationResult(validationResults,
					GetMessage(objectToValidate, key),
					currentTarget,
					key);
			}
		}

		/// <summary>
		/// Gets the message representing a failed validation.
		/// </summary>
		/// <param name="objectToValidate">The object for which validation was performed.</param>
		/// <param name="key">The key representing the value being validated for <paramref name="objectToValidate"/>.</param>
		/// <returns>The message representing the validation failure.</returns>
		protected override string GetMessage(object objectToValidate, string key)
		{
			return string.Format(CultureInfo.CurrentUICulture,
				this.MessageTemplate,
				objectToValidate,
				key,
				this.Tag,
				this.lowerBound,
				this.LowerUnit,
				this.upperBound,
				this.UpperUnit);
		}

		/// <summary>
		/// Gets the Default Message Template when the validator is non negated.
		/// </summary>
		protected override string DefaultNonNegatedMessageTemplate
		{
			get { return Resources.RelativeDateTimeNonNegatedDefaultMessageTemplate; }
		}

		/// <summary>
		/// Gets the Default Message Template when the validator is negated.
		/// </summary>
		protected override string DefaultNegatedMessageTemplate
		{
			get { return Resources.RelativeDateTimeNegatedDefaultMessageTemplate; }
		}

		#region test only properties

		internal int LowerBound
		{
			get { return lowerBound; }
		}

		internal DateTimeUnit LowerUnit
		{
			get { return this.lowerUnit; }
		}

		internal RangeBoundaryType LowerBoundType
		{
			get { return rangeChecker.LowerBoundType; }
		}

		internal int UpperBound
		{
			get { return upperBound; }
		}

		internal DateTimeUnit UpperUnit
		{
			get { return this.upperUnit; }
		}

		internal RangeBoundaryType UpperBoundType
		{
			get { return rangeChecker.UpperBoundType; }
		}

		internal RelativeDateTimeGenerator Generator
		{
			get { return generator; }
		}

		#endregion
	}
}

