/*
 * CVS identifier:
 *
 * $Id: WaveletFilter.java,v 1.13 2001/08/02 11:11:16 grosbois Exp $
 *
 * Class:                   WaveletFilter
 *
 * Description:             Defines the interface for WT filters (analysis
 *                          and synthesis)
 *
 *
 *
 * COPYRIGHT:
 * 
 * This software module was originally developed by Rapha�l Grosbois and
 * Diego Santa Cruz (Swiss Federal Institute of Technology-EPFL); Joel
 * Askel�f (Ericsson Radio Systems AB); and Bertrand Berthelot, David
 * Bouchard, F�lix Henry, Gerard Mozelle and Patrice Onno (Canon Research
 * Centre France S.A) in the course of development of the JPEG2000
 * standard as specified by ISO/IEC 15444 (JPEG 2000 Standard). This
 * software module is an implementation of a part of the JPEG 2000
 * Standard. Swiss Federal Institute of Technology-EPFL, Ericsson Radio
 * Systems AB and Canon Research Centre France S.A (collectively JJ2000
 * Partners) agree not to assert against ISO/IEC and users of the JPEG
 * 2000 Standard (Users) any of their rights under the copyright, not
 * including other intellectual property rights, for this software module
 * with respect to the usage by ISO/IEC and Users of this software module
 * or modifications thereof for use in hardware or software products
 * claiming conformance to the JPEG 2000 Standard. Those intending to use
 * this software module in hardware or software products are advised that
 * their use may infringe existing patents. The original developers of
 * this software module, JJ2000 Partners and ISO/IEC assume no liability
 * for use of this software module or modifications thereof. No license
 * or right to this software module is granted for non JPEG 2000 Standard
 * conforming products. JJ2000 Partners have full right to use this
 * software module for his/her own purpose, assign or donate this
 * software module to any third party and to inhibit third parties from
 * using this software module for non JPEG 2000 Standard conforming
 * products. This copyright notice must be included in all copies or
 * derivative works of this software module.
 * 
 * Copyright (c) 1999/2000 JJ2000 Partners.
 */
namespace jpeg2000_decoder.Wavelet
{
/**
 * This interface defines how a wavelet filter implementation should present
 * itself. This interface defines only the commonalities between the analysis
 * and synthesis filters. The AnWTFilter and SynWTFilter classes provide the
 * specifics of analysis and synthesis filters.
 *
 * <p>Both analysis and filters must be able to return the extent of the
 * negative and positive support for both synthesis and analysis sides. This
 * simplifies the sue of some functionalities that need extra information
 * about the filters.</p>
 *
 * @see jj2000.j2k.wavelet.analysis.AnWTFilter
 *
 * @see jj2000.j2k.wavelet.synthesis.SynWTFilter
 * */
public abstract class WaveletFilter {

    /** The ID for integer lifting spteps implementations */
    public const int WT_FILTER_INT_LIFT = 0;

    /** The ID for floating-point lifting spteps implementations */
    public const int WT_FILTER_FLOAT_LIFT = 1;

    /** The ID for floatring-poitn convolution implementations */
    public const int WT_FILTER_FLOAT_CONVOL = 2;

    /**
     * Returns the negative support of the low-pass analysis filter. That is
     * the number of taps of the filter in the negative direction.
     *
     * @return The number of taps of the low-pass analysis filter in the
     * negative direction */
    public abstract int getAnLowNegSupport();

    /**
     * Returns the positive support of the low-pass analysis filter. That is
     * the number of taps of the filter in the negative direction.
     *
     * @return The number of taps of the low-pass analysis filter in the
     * positive direction */
    public abstract int getAnLowPosSupport();

    /**
     * Returns the negative support of the high-pass analysis filter. That is
     * the number of taps of the filter in the negative direction.
     *
     * @return The number of taps of the high-pass analysis filter in the
     * negative direction */
    public abstract int getAnHighNegSupport();

    /**
     * Returns the positive support of the high-pass analysis filter. That is
     * the number of taps of the filter in the negative direction.
     *
     * @return The number of taps of the high-pass analysis filter in
     * the positive direction */
    public abstract int getAnHighPosSupport();

    /**
     * Returns the negative support of the low-pass synthesis filter. That is
     * the number of taps of the filter in the negative direction.
     *
     * @return The number of taps of the low-pass synthesis filter in the
     * negative direction */
    public abstract int getSynLowNegSupport();

    /**
     * Returns the positive support of the low-pass synthesis filter. That is
     * the number of taps of the filter in the negative direction.
     *
     * @return The number of taps of the low-pass synthesis filter in the
     * positive direction */
    public abstract int getSynLowPosSupport();

    /**
     * Returns the negative support of the high-pass synthesis filter. That is
     * the number of taps of the filter in the negative direction.
     *
     * @return The number of taps of the high-pass synthesis filter in the
     * negative direction */
    public abstract int getSynHighNegSupport();

    /**
     * Returns the positive support of the high-pass synthesis filter. That is
     * the number of taps of the filter in the negative direction.
     *
     * @return The number of taps of the high-pass synthesis filter in the
     * positive direction */
    public abstract int getSynHighPosSupport();

    /**
     * Returns the implementation type of this filter, as defined in this
     * class, such as WT_FILTER_INT_LIFT, WT_FILTER_FLOAT_LIFT,
     * WT_FILTER_FLOAT_CONVOL.
     *
     * @return The implementation type of this filter: WT_FILTER_INT_LIFT,
     * WT_FILTER_FLOAT_LIFT, WT_FILTER_FLOAT_CONVOL.  */
    public abstract int getImplType();

    /**
     * Returns the type of data on which this filter works, as defined in the
     * DataBlk interface.
     *
     * @return The type of data as defined in the DataBlk interface.
     *
     * @see jj2000.j2k.image.DataBlk */
    public abstract int getDataType();

    /**
     * Returns the reversibility of the filter. A filter is considered
     * reversible if it is suitable for lossless coding.
     *
     * @return true if the filter is reversible, false otherwise.
     * */
    public abstract bool isReversible();

    /**
     * Returns true if the wavelet filter computes or uses the same "inner"
     * subband coefficient as the full frame wavelet transform, and false
     * otherwise. In particular, for block based transforms with reduced
     * overlap, this method should return false. The term "inner" indicates
     * that this applies only with respect to the coefficient that are not
     * affected by image boundaries processings such as symmetric extension,
     * since there is not reference method for this.
     *
     * <p>The result depends on the length of the allowed overlap when
     * compared to the overlap required by the wavelet filter. It also depends
     * on how overlap processing is implemented in the wavelet filter.</p>
     *
     * @param tailOvrlp This is the number of samples in the input signal
     * before the first sample to filter that can be used for overlap.
     *
     * @param headOvrlp This is the number of samples in the input signal
     * after the last sample to filter that can be used for overlap.
     *
     * @param inLen This is the lenght of the input signal to filter.The
     * required number of samples in the input signal after the last sample
     * depends on the length of the input signal.
     *
     * @return true if the overlaps are large enough and correct processing is
     * performed, false otherwise.
     * */
    public abstract bool isSameAsFullWT(int tailOvrlp, int headOvrlp, int inLen);
}
}