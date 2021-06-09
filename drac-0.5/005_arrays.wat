;; WebAssembly text format code generated by the drac compiler.

(module
	(import "drac" "printi" (func $printi (param i32) (result i32)))
	(import "drac" "printc" (func $printc (param i32) (result i32)))
	(import "drac" "prints" (func $prints (param i32) (result i32)))
	(import "drac" "println" (func $println (result i32)))
	(import "drac" "readi" (func $readi (result i32)))
	(import "drac" "reads" (func $reads (result i32)))
	(import "drac" "new" (func $new (param i32) (result i32)))
	(import "drac" "size" (func $size (param i32) (result i32)))
	(import "drac" "add" (func $add (param i32 i32) (result i32)))
	(import "drac" "get" (func $get (param i32 i32) (result i32)))
	(import "drac" "set" (func $set (param i32 i32 i32) (result i32)))

	(func $print_array
		(param $a i32)
		(result i32)
		(local $_temp i32)
		(local $s i32)
		(local $first i32)
		(local $i i32)
		(local $n i32)
    i32.const 1
		local.set $first
		i32.const 0
		call $new
		local.set $_temp
		local.get $_temp
		local.get $_temp
		i32.const 91
		call $add
		drop
		call $printc
		drop
		i32.const 0
		local.set $i
		local.get $a
		call $size
		local.set $n
		local.get $first
		if
    i32.const 0
		local.set $first
		else
		i32.const 0
		call $new
		local.set $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		i32.const 44
		call $add
		drop
		i32.const 32
		call $add
		drop
		call $prints
		drop
		end
		local.get $a
		local.get $i
		call $get
		call $printi
		drop
		local.get $i
		i32.const 1
		i32.add
		local.set $i
		i32.const 0
		call $new
		local.set $_temp
		local.get $_temp
		local.get $_temp
		i32.const 93
		call $add
		drop
		call $printc
		drop
		i32.const 0
	)
	(func $sum_array
		(param $a i32)
		(result i32)
		(local $_temp i32)
		(local $s i32)
		(local $sum i32)
		(local $i i32)
		(local $n i32)
		i32.const 0
		local.set $sum
		i32.const 0
		local.set $i
		local.get $a
		call $size
		local.set $n
		local.get $sum
		local.get $a
		local.get $i
		call $get
		i32.add
		local.set $sum
		local.get $i
		i32.const 1
		i32.add
		local.set $i
		local.get $sum
		return
		i32.const 0
	)
	(func $max_array
		(param $a i32)
		(result i32)
		(local $_temp i32)
		(local $s i32)
		(local $max i32)
		(local $i i32)
		(local $n i32)
		(local $x i32)
		local.get $a
		i32.const 0
		call $get
		local.set $max
		i32.const 0
		local.set $i
		local.get $a
		call $size
		local.set $n
		local.get $a
		local.get $i
		call $get
		local.set $x
		local.get $x
		local.get $max
		i32.gt_s
		if
		local.get $x
		local.set $max
		end
		local.get $i
		i32.const 1
		i32.add
		local.set $i
		local.get $max
		return
		i32.const 0
	)
	(func $sort_array
		(param $a i32)
		(result i32)
		(local $_temp i32)
		(local $s i32)
		(local $i i32)
		(local $j i32)
		(local $t i32)
		(local $n i32)
		(local $swap i32)
		local.get $a
		call $size
		local.set $n
		i32.const 0
		local.set $i
		i32.const 0
		local.set $j
    i32.const 0
		local.set $swap
		local.get $a
		local.get $j
		call $get
		local.get $a
		local.get $j
		i32.const 1
		i32.add
		call $get
		i32.gt_s
		if
		local.get $a
		local.get $j
		call $get
		local.set $t
		local.get $a
		local.get $j
		local.get $a
		local.get $j
		i32.const 1
		i32.add
		call $get
		call $set
		drop
		local.get $a
		local.get $j
		i32.const 1
		i32.add
		local.get $t
		call $set
		drop
    i32.const 1
		local.set $swap
		end
		local.get $j
		i32.const 1
		i32.add
		local.set $j
		local.get $swap
		i32.eqz
		if
		end
		local.get $i
		i32.const 1
		i32.add
		local.set $i
		i32.const 0
	)
	(func
		(export "main")
		(result i32)
		(local $_temp i32)
		(local $s i32)
		(local $array i32)
		(local $sum i32)
		(local $max i32)
		local.set $array
		i32.const 0
		call $new
		local.set $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		i32.const 79
		call $add
		drop
		i32.const 114
		call $add
		drop
		i32.const 105
		call $add
		drop
		i32.const 103
		call $add
		drop
		i32.const 105
		call $add
		drop
		i32.const 110
		call $add
		drop
		i32.const 97
		call $add
		drop
		i32.const 108
		call $add
		drop
		i32.const 32
		call $add
		drop
		i32.const 97
		call $add
		drop
		i32.const 114
		call $add
		drop
		i32.const 114
		call $add
		drop
		i32.const 97
		call $add
		drop
		i32.const 121
		call $add
		drop
		i32.const 58
		call $add
		drop
		i32.const 32
		call $add
		drop
		call $prints
		drop
		local.get $array
		call $print_array
		drop
		call $println
		drop
		local.get $array
		call $sum_array
		local.set $sum
		local.get $array
		call $max_array
		local.set $max
		i32.const 0
		call $new
		local.set $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		i32.const 83
		call $add
		drop
		i32.const 117
		call $add
		drop
		i32.const 109
		call $add
		drop
		i32.const 32
		call $add
		drop
		i32.const 111
		call $add
		drop
		i32.const 102
		call $add
		drop
		i32.const 32
		call $add
		drop
		i32.const 97
		call $add
		drop
		i32.const 114
		call $add
		drop
		i32.const 114
		call $add
		drop
		i32.const 97
		call $add
		drop
		i32.const 121
		call $add
		drop
		i32.const 58
		call $add
		drop
		i32.const 32
		call $add
		drop
		i32.const 32
		call $add
		drop
		i32.const 32
		call $add
		drop
		call $prints
		drop
		local.get $sum
		call $printi
		drop
		call $println
		drop
		i32.const 0
		call $new
		local.set $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		i32.const 77
		call $add
		drop
		i32.const 97
		call $add
		drop
		i32.const 120
		call $add
		drop
		i32.const 32
		call $add
		drop
		i32.const 111
		call $add
		drop
		i32.const 102
		call $add
		drop
		i32.const 32
		call $add
		drop
		i32.const 97
		call $add
		drop
		i32.const 114
		call $add
		drop
		i32.const 114
		call $add
		drop
		i32.const 97
		call $add
		drop
		i32.const 121
		call $add
		drop
		i32.const 58
		call $add
		drop
		i32.const 32
		call $add
		drop
		i32.const 32
		call $add
		drop
		i32.const 32
		call $add
		drop
		call $prints
		drop
		local.get $max
		call $printi
		drop
		call $println
		drop
		local.get $array
		call $sort_array
		drop
		i32.const 0
		call $new
		local.set $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		local.get $_temp
		i32.const 83
		call $add
		drop
		i32.const 111
		call $add
		drop
		i32.const 114
		call $add
		drop
		i32.const 116
		call $add
		drop
		i32.const 101
		call $add
		drop
		i32.const 100
		call $add
		drop
		i32.const 32
		call $add
		drop
		i32.const 97
		call $add
		drop
		i32.const 114
		call $add
		drop
		i32.const 114
		call $add
		drop
		i32.const 97
		call $add
		drop
		i32.const 121
		call $add
		drop
		i32.const 58
		call $add
		drop
		i32.const 32
		call $add
		drop
		i32.const 32
		call $add
		drop
		i32.const 32
		call $add
		drop
		call $prints
		drop
		local.get $array
		call $print_array
		drop
		call $println
		drop
		i32.const 0
	)
)
