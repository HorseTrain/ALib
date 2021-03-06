using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.LowLevel
{
    public enum LowLevelClassNames
    {
        Unallocated,
        SVE_bitwise_logical_operations_predicated,
        SVE_integer_add_subtract_vectors_predicated,
        SVE_integer_divide_vectors_predicated,
        SVE_integer_min_max_difference_predicated,
        SVE_integer_multiply_vectors_predicated,
        SVE_bitwise_logical_reduction_predicated,
        SVE_constructive_prefix_predicated,
        SVE_integer_add_reduction_predicated,
        SVE_integer_min_max_reduction_predicated,
        SVE_bitwise_shift_by_immediate_predicated,
        SVE_bitwise_shift_by_vector_predicated,
        SVE_bitwise_shift_by_wide_elements_predicated,
        Barriers,
        Compare_and_branch_immediate,
        Conditional_branch_immediate,
        Exception_generation,
        Hints,
        PSTATE,
        System_instructions,
        System_instructions_with_register_argument,
        System_register_move,
        Test_and_branch_immediate,
        Unconditional_branch_immediate,
        Unconditional_branch_register,
        SVE_bitwise_unary_operations_predicated,
        SVE_integer_unary_operations_predicated,
        Advanced_SIMD_load_store_multiple_structures,
        Advanced_SIMD_load_store_multiple_structures_post_indexed,
        Advanced_SIMD_load_store_single_structure,
        Advanced_SIMD_load_store_single_structure_post_indexed,
        Atomic_memory_operations,
        Compare_and_swap,
        Compare_and_swap_pair,
        LDAPR_STLR_unscaled_immediate,
        Load_register_literal,
        Load_store_exclusive_pair,
        Load_store_exclusive_register,
        Load_store_memory_tags,
        Load_store_no_allocate_pair_offset,
        Load_store_ordered,
        Load_store_register_immediate_post_indexed,
        Load_store_register_immediate_pre_indexed,
        Load_store_register_pac,
        Load_store_register_register_offset,
        Load_store_register_unprivileged,
        Load_store_register_unscaled_immediate,
        Load_store_register_unsigned_immediate,
        Load_store_register_pair_offset,
        Load_store_register_pair_post_indexed,
        Load_store_register_pair_pre_indexed,
        SVE_integer_multiply_accumulate_writing_addend_predicated,
        SVE_integer_multiply_add_writing_multiplicand_predicated,
        SVE_integer_add_subtract_vectors_unpredicated,
        Add_subtract_immediate,
        Add_subtract_immediate_with_tags,
        Bitfield,
        Extract,
        Logical_immediate,
        Move_wide_immediate,
        PC_rel__addressing,
        SVE_bitwise_logical_operations_unpredicated,
        SVE_index_generation_immediate_start_immediate_increment,
        SVE_index_generation_immediate_start_register_increment,
        SVE_index_generation_register_start_immediate_increment,
        SVE_index_generation_register_start_register_increment,
        SVE_stack_frame_adjustment,
        SVE_stack_frame_size,
        Add_subtract_extended_register,
        Add_subtract_shifted_register,
        Add_subtract_with_carry,
        Conditional_compare_immediate,
        Conditional_compare_register,
        Conditional_select,
        Data_processing_1_source,
        Data_processing_2_source,
        Data_processing_3_source,
        Evaluate_into_flags,
        Logical_shifted_register,
        Rotate_right_into_flags,
        SVE_bitwise_shift_by_immediate_unpredicated,
        SVE_bitwise_shift_by_wide_elements_unpredicated,
        Advanced_SIMD_across_lanes,
        Advanced_SIMD_copy,
        Advanced_SIMD_extract,
        Advanced_SIMD_modified_immediate,
        Advanced_SIMD_permute,
        Advanced_SIMD_scalar_copy,
        Advanced_SIMD_scalar_pairwise,
        Advanced_SIMD_scalar_shift_by_immediate,
        Advanced_SIMD_scalar_three_different,
        Advanced_SIMD_scalar_three_same,
        Advanced_SIMD_scalar_three_same_FP16,
        Advanced_SIMD_scalar_three_same_extra,
        Advanced_SIMD_scalar_two_register_miscellaneous,
        Advanced_SIMD_scalar_two_register_miscellaneous_FP16,
        Advanced_SIMD_scalar_x_indexed_element,
        Advanced_SIMD_shift_by_immediate,
        Advanced_SIMD_table_lookup,
        Advanced_SIMD_three_different,
        Advanced_SIMD_three_same,
        Advanced_SIMD_three_same_FP16,
        Advanced_SIMD_three_register_extension,
        Advanced_SIMD_two_register_miscellaneous,
        Advanced_SIMD_two_register_miscellaneous_FP16,
        Advanced_SIMD_vector_x_indexed_element,
        Conversion_between_floating_point_and_fixed_point,
        Conversion_between_floating_point_and_integer,
        Cryptographic_AES,
        Cryptographic_four_register,
        Cryptographic_three_register_SHA,
        Cryptographic_three_register_SHA_512,
        Cryptographic_three_register_imm2,
        Cryptographic_three_register_imm6,
        Cryptographic_two_register_SHA,
        Cryptographic_two_register_SHA_512,
        Floating_point_compare,
        Floating_point_conditional_compare,
        Floating_point_conditional_select,
        Floating_point_data_processing_1_source,
        Floating_point_data_processing_2_source,
        Floating_point_data_processing_3_source,
        Floating_point_immediate,
        SVE_address_generation,
        Reserved,
        SVE_constructive_prefix_unpredicated,
        SVE_floating_point_exponential_accelerator,
        SVE_floating_point_trig_select_coefficient,
        SVE_element_count,
        SVE_inc_dec_register_by_element_count,
        SVE_inc_dec_vector_by_element_count,
        SVE_saturating_inc_dec_register_by_element_count,
        SVE_saturating_inc_dec_vector_by_element_count,
        SVE_extract_vector_immediate_offset_destructive,
        SVE_permute_vector_segments,
        SVE_bitwise_logical_with_immediate_unpredicated,
        SVE_broadcast_bitmask_immediate,
        SVE_copy_floating_point_immediate_predicated,
        SVE_copy_integer_immediate_predicated,
        SVE_broadcast_indexed_element,
        SVE_table_lookup,
        SVE_broadcast_general_register,
        SVE_insert_SIMD_FP_scalar_register,
        SVE_insert_general_register,
        SVE_reverse_vector_elements,
        SVE_unpack_vector_elements,
        SVE_permute_predicate_elements,
        SVE_reverse_predicate_elements,
        SVE_unpack_predicate_elements,
        SVE_permute_vector_elements,
        SVE_compress_active_elements,
        SVE_conditionally_broadcast_element_to_vector,
        SVE_conditionally_extract_element_to_SIMD_FP_scalar,
        SVE_conditionally_extract_element_to_general_register,
        SVE_copy_SIMD_FP_scalar_register_to_vector_predicated,
        SVE_copy_general_register_to_vector_predicated,
        SVE_extract_element_to_SIMD_FP_scalar_register,
        SVE_extract_element_to_general_register,
        SVE_reverse_within_elements,
        SVE_vector_splice_destructive,
        SVE_select_vector_elements_predicated,
        SVE_integer_compare_vectors,
        SVE_integer_compare_with_wide_elements,
        SVE_integer_compare_with_unsigned_immediate,
        SVE_predicate_logical_operations,
        SVE_propagate_break_from_previous_partition,
        SVE_partition_break_condition,
        SVE_propagate_break_to_next_partition,
        SVE_predicate_first_active,
        SVE_predicate_initialize,
        SVE_predicate_next_active,
        SVE_predicate_read_from_FFR_predicated,
        SVE_predicate_read_from_FFR_unpredicated,
        SVE_predicate_test,
        SVE_predicate_zero,
        SVE_integer_compare_with_signed_immediate,
        SVE_predicate_count,
        SVE_inc_dec_register_by_predicate_count,
        SVE_inc_dec_vector_by_predicate_count,
        SVE_saturating_inc_dec_register_by_predicate_count,
        SVE_saturating_inc_dec_vector_by_predicate_count,
        SVE_FFR_initialise,
        SVE_FFR_write_from_predicate,
        SVE_conditionally_terminate_scalars,
        SVE_integer_compare_scalar_count_and_limit,
        SVE_broadcast_floating_point_immediate_unpredicated,
        SVE_broadcast_integer_immediate_unpredicated,
        SVE_integer_add_subtract_immediate_unpredicated,
        SVE_integer_min_max_immediate_unpredicated,
        SVE_integer_multiply_immediate_unpredicated,
        SVE_integer_dot_product_unpredicated,
        SVE_mixed_sign_dot_product,
        SVE_integer_dot_product_indexed,
        SVE_mixed_sign_dot_product_indexed,
        SVE_integer_matrix_multiply_accumulate,
        SVE_floating_point_complex_add_predicated,
        SVE_floating_point_convert_precision_odd_elements,
        SVE_floating_point_complex_multiply_add_predicated,
        SVE_floating_point_multiply_add_indexed,
        SVE_floating_point_complex_multiply_add_indexed,
        SVE_floating_point_multiply_indexed,
        SVE_BFloat16_floating_point_dot_product_indexed,
        SVE_floating_point_multiply_add_long_indexed,
        SVE_BFloat16_floating_point_dot_product,
        SVE_floating_point_multiply_add_long,
        SVE_floating_point_matrix_multiply_accumulate,
        SVE_floating_point_recursive_reduction,
        SVE_floating_point_reciprocal_estimate_unpredicated,
        SVE_floating_point_compare_with_zero,
        SVE_floating_point_serial_reduction_predicated,
        SVE_floating_point_arithmetic_unpredicated,
        SVE_floating_point_arithmetic_predicated,
        SVE_floating_point_arithmetic_with_immediate_predicated,
        SVE_floating_point_trig_multiply_add_coefficient,
        SVE_floating_point_convert_precision,
        SVE_floating_point_convert_to_integer,
        SVE_floating_point_round_to_integral_value,
        SVE_floating_point_unary_operations,
        SVE_integer_convert_to_floating_point,
        SVE_floating_point_compare_vectors,
        SVE_floating_point_multiply_accumulate_writing_addend,
        SVE_floating_point_multiply_accumulate_writing_multiplicand,
        SVE_32_bit_gather_load_scalar_plus_32_bit_unscaled_offsets,
        SVE_32_bit_gather_load_vector_plus_immediate,
        SVE_32_bit_gather_load_halfwords_scalar_plus_32_bit_scaled_offsets,
        SVE_32_bit_gather_load_words_scalar_plus_32_bit_scaled_offsets,
        SVE_32_bit_gather_prefetch_scalar_plus_32_bit_scaled_offsets,
        SVE_32_bit_gather_prefetch_vector_plus_immediate,
        SVE_contiguous_prefetch_scalar_plus_immediate,
        SVE_contiguous_prefetch_scalar_plus_scalar,
        SVE_load_and_broadcast_element,
        SVE_load_predicate_register,
        SVE_load_vector_register,
        SVE_contiguous_first_fault_load_scalar_plus_scalar,
        SVE_contiguous_load_scalar_plus_immediate,
        SVE_contiguous_load_scalar_plus_scalar,
        SVE_contiguous_non_fault_load_scalar_plus_immediate,
        SVE_contiguous_non_temporal_load_scalar_plus_immediate,
        SVE_contiguous_non_temporal_load_scalar_plus_scalar,
        SVE_load_and_broadcast_quadword_scalar_plus_immediate,
        SVE_load_and_broadcast_quadword_scalar_plus_scalar,
        SVE_load_multiple_structures_scalar_plus_immediate,
        SVE_load_multiple_structures_scalar_plus_scalar,
        SVE_64_bit_gather_load_scalar_plus_32_bit_unpacked_scaled_offsets,
        SVE_64_bit_gather_load_scalar_plus_64_bit_scaled_offsets,
        SVE_64_bit_gather_load_scalar_plus_64_bit_unscaled_offsets,
        SVE_64_bit_gather_load_scalar_plus_unpacked_32_bit_unscaled_offsets,
        SVE_64_bit_gather_load_vector_plus_immediate,
        SVE_64_bit_gather_prefetch_scalar_plus_64_bit_scaled_offsets,
        SVE_64_bit_gather_prefetch_scalar_plus_unpacked_32_bit_scaled_offsets,
        SVE_64_bit_gather_prefetch_vector_plus_immediate,
        SVE_contiguous_store_scalar_plus_scalar,
        SVE_store_predicate_register,
        SVE_store_vector_register,
        SVE_contiguous_non_temporal_store_scalar_plus_scalar,
        SVE_store_multiple_structures_scalar_plus_scalar,
        SVE_32_bit_scatter_store_vector_plus_immediate,
        SVE_64_bit_scatter_store_scalar_plus_64_bit_scaled_offsets,
        SVE_64_bit_scatter_store_scalar_plus_64_bit_unscaled_offsets,
        SVE_64_bit_scatter_store_vector_plus_immediate,
        SVE_contiguous_non_temporal_store_scalar_plus_immediate,
        SVE_contiguous_store_scalar_plus_immediate,
        SVE_store_multiple_structures_scalar_plus_immediate,
        SVE_32_bit_scatter_store_scalar_plus_32_bit_scaled_offsets,
        SVE_32_bit_scatter_store_scalar_plus_32_bit_unscaled_offsets,
        SVE_64_bit_scatter_store_scalar_plus_unpacked_32_bit_scaled_offsets,
        SVE_64_bit_scatter_store_scalar_plus_unpacked_32_bit_unscaled_offsets,
    }
}
